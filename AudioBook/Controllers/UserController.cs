using AudioBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AudioBook.Controllers
{
    public class UserController : Controller
    {
        private ModelContext db = new ModelContext();

        [HttpGet]
        public ActionResult Register()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "AudioBook");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Register([Bind(Exclude = "Status,ActivationCode")]User user)
        {
            bool status = false;
            string message = "";
            if (ModelState.IsValid)
            {

                #region //Email is alredy Exist
                var isExistEmail = IsEmailExsist(user.Email);
                if (isExistEmail)
                {
                    ModelState.AddModelError("EmailExist", "Email alredy exist");
                    return View(user);
                }
                #endregion

                #region //Username is alredy Exist
                var isExistUsername = IsEmailExsist(user.Email);
                if (isExistUsername)
                {
                    ModelState.AddModelError("UsernameExist", "Username alredy exist");
                    return View(user);
                }
                #endregion

                #region Generate Activation code 
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region Password hashing 
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion

                user.Status = false;

                #region Save data to database
                db.Users.Add(user);
                db.SaveChanges();


                // Send email to user
                SendVerificationLinkEmail(user.Email, user.ActivationCode.ToString());
                message = "Registeri succesfuly done , Acount activate" +
                    " has ben send to email id : " + user.Email;
                status = true;
                #endregion

            }
            else
            {
                message = "Invalid Request";
            }

            ViewBag.Message = message;
            ViewBag.Status = status;


            return View(user);
        }

        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {

            Guid newGuid;

            if (id == null || !Guid.TryParse(id, out newGuid))
            {
                return RedirectToAction("Index", "AudioBook");
            }

            bool status = false;

            db.Configuration.ValidateOnSaveEnabled = false;

            var v = db.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
            if (v != null && !v.Status)
            {
                v.Status = true;
                db.SaveChanges();
                status = true;
            }
            else
            {
                return RedirectToAction("Index", "AudioBook");
            }

            ViewBag.Status = status;
            return View();
        }

        //Login
        [HttpGet]
        public ActionResult Login()
        {

            if (Session["admin"] != null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "AudioBook");
            }
            else
            {
                return View();
            }

        }
        //Login Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string returnUrl = "")
        {
            bool status = false;
            string message = "";

            if (login.EmailOrUsername != null && login.password != null && login.EmailOrUsername.Length != 0 && login.password.Length != 0)
            {
                var v = db.Users.Where(a => a.Email == login.EmailOrUsername || a.UserName == login.EmailOrUsername).FirstOrDefault();

                if (v != null)
                {

                    if (string.Compare(Crypto.Hash(login.password), v.Password) == 0)
                    {

                        if (v.Status == true)
                        {
                            if (v.RoleId == 1 || v.RoleId == 2)
                            {
                                Session["admin"] = true;
                                if (v.RoleId == 1)
                                    Session["adminRoleId"] = 1;
                                if (v.RoleId == 2)
                                    Session["adminRoleId"] = 2;

                                return RedirectToAction("Index", "Admin");
                            }
                            if (v.RoleId == 0)
                            {
                                int timeout = login.RememberMe ? 60 : 8;
                                String cookieName = login.EmailOrUsername + "|" + v.ImgSrc;
                                var ticket = new FormsAuthenticationTicket(cookieName, login.RememberMe, timeout);
                                string encrypted = FormsAuthentication.Encrypt(ticket);
                                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                                cookie.Expires = DateTime.Now.AddMinutes(timeout);
                                cookie.HttpOnly = true;
                                Response.Cookies.Add(cookie);

                                if (Url.IsLocalUrl(returnUrl))
                                {
                                    return Redirect(returnUrl);
                                }
                                else
                                {
                                    return RedirectToAction("Index", "AudioBook");
                                }
                            }
                        }
                        else
                        {
                            message = "Your account is not actived";
                            status = true;
                        }
                    }
                    else
                    {
                        message = "Invalid credential provided";
                        status = true;
                    }
                }
                else
                {
                    message = "Invalid credential provided";
                    status = true;
                }
            }


            ViewBag.Status = status;
            ViewBag.Message = message;
            return View();
        }

        //Logout

        [HttpPost]
        public ActionResult Logout()
        {

            Session["admin"] = null;
            Session.Clear();
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "AudioBook");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        //Index only SuperAdmin
        [HttpGet]
        public ActionResult UserIndex()
        {
            if (Session["admin"] != null)
            {
                return View(db.Users.ToList().Where(a => a.RoleId == 0));
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult AdminIndex()
        {
            if (Session["admin"] != null && Convert.ToInt32(Session["adminRoleId"]) == 2)
            {
                return View(db.Users.ToList().Where(a => a.RoleId == 1));
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (Session["admin"] != null && Convert.ToInt32(Session["adminRoleId"]) == 2)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                User user = db.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }

            return RedirectToAction("Index", "AudioBook");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (Session["admin"] != null && Convert.ToInt32(Session["adminRoleId"]) == 2)
            {
                if (user.RoleId == 0 || user.RoleId == 1)
                {

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    if (user.RoleId == 0) return RedirectToAction("UserIndex");
                    if (user.RoleId == 1) return RedirectToAction("AdminIndex");


                    return View(user);
                }
            }

            return RedirectToAction("Index", "AudioBook");
        }

        //public ActionResult Profil(string userName)
        //{
        //    if (userName == null)
        //    {
        //        return RedirectToAction("Index", "AudioBook");
        //    }

        //    var v = db.Users.Where(x => x.UserName == userName).FirstOrDefault();

        //    if (v == null)
        //    {
        //        return RedirectToAction("Index", "AudioBook");
        //    }



        //    return View(v);
        //}

        public ActionResult UserProfilEdit()
        {
            return View();
        }

        public ActionResult UserProfilView()
        {
            return View();
        }

        public ActionResult UserProfilChangeProfilePhoto()
        {
            return View();
        }

        [NonAction]
        public bool IsEmailExsist(string EmailId)
        {

            var v = db.Users.Where(a => a.Email == EmailId).FirstOrDefault();
            return v != null;

        }

        [NonAction]
        public bool IsUsernameExsist(string username)
        {
            var v = db.Users.Where(a => a.UserName == username).FirstOrDefault();

            return v != null;

        }

        [NonAction]
        public void SendVerificationLinkEmail(string EmailID, string activationCode)
        {
            var verifyUrl = "/User/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("vezir.tarverdi@gmail.com", "AudioBook");
            var toEmail = new MailAddress(EmailID);

            var fromEmailPassword = "vezir313"; // Requsest with actual password
            string subject = "Your account is succesfuly created!";
            string body = "<br/> <br>  We are excited to tell you that your Donet Awesome is" +
                " succesfuly created. Please click on the below link to verify your acount" +
                "<br/> <br> <a href='" + link + "'>" + link + "</a> ";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {

                smtp.Send(message);
            }
        }

    }

}