using AudioBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AudioBook.Controllers
{
    public class AdminController : Controller
    {

        private ModelContext db = new ModelContext();

        // GET: Admin
        public ActionResult Index()
        {
            if(Session["admin"] != null)
            {
                ViewBag.Books = db.Books.ToList();
                ViewBag.Genres = db.Genres.ToList();
                ViewBag.Writers = db.Writers.ToList();
                ViewBag.Language = db.Languages.ToList();
                ViewBag.Books = db.Books.ToList();
                ViewBag.Users = db.Users.ToList();
                return View();
            }
            else
            {
                return HttpNotFound();
            }

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}