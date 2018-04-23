using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AudioBook.Models;
using System.IO;
using System.Linq.Dynamic;

namespace AudioBook.Controllers
{
    public class BooksController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Books
        public ActionResult Index(int page=1,string sort="Title",string sortdir="asc",string search="")
        {
            int pageSize = 10;
            int totalRecord = 0;
            if (page<1)
            {
                page = 1;
            }
            int skip = (page * pageSize) - pageSize;
            var data = GetBooks(search, sort, sortdir, skip, pageSize, out totalRecord);
            ViewBag.search = search;
            if (Session["admin"] != null)
            {
                //var books = db.Books.Include(b => b.Language).Include(b => b.Writer);
                //return View(books.ToList());
                return View(data);
            }
            else
            {
                return HttpNotFound();
            }

        }
        public List<Book> GetBooks(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
                        var v = (from a in db.Books
                                 where
                                 a.Title.Contains(search)
                                 select a);
            totalRecord = v.Count();
            v = v.OrderBy(sort + " " +sortdir);
            if (pageSize>0)
            {
                v=v.Skip(skip).Take(pageSize);
            }
            return v.ToList();
        }
        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["admin"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Book book = db.Books.Find(id);
                if (book == null)
                {
                    return HttpNotFound();
                }


                var sounds = db.Sounds.ToList().Where(a => a.BookId == book.Id);
                ViewBag.Sounds = sounds;
                return View(book);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            if (Session["admin"] != null)
            {
                ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name");
                ViewBag.WriterId = new SelectList(db.Writers, "Id", "Name");
                return View();
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Info,ImgSrc,Price,DownloadCount,FavoriteCount,WriterId,LanguageId,Status")] Book book,IEnumerable<HttpPostedFileBase> files)
        {

            if (Session["admin"] != null)
            {
                int i = 1;
                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string path = Guid.NewGuid() + Path.GetExtension(file.FileName);

                        Sound sound = new Sound();
                        sound.name = book.Title + "-" + i;
                        sound.row = i;
                        i++;
                        sound.SoundSource = path;
                        sound.Book = book;
                        sound.BookId = book.Id;
                        db.Sounds.Add(sound);

                        file.SaveAs(Path.Combine(Server.MapPath("~/Uploads/Books/mp3/"), path));
                    }
                }


                var inputPhoto = HttpContext.Request.Files["ImgSrc"];
                if (inputPhoto.ContentType == "image/gif" || inputPhoto.ContentType == "image/png" || inputPhoto.ContentType == "image/jpeg")
                {
                    string fileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff-") + inputPhoto.FileName;
                    string newFile = Path.Combine(HttpContext.Server.MapPath("~/Uploads/Books/img/"), fileName);
                    inputPhoto.SaveAs(newFile);
                    book.ImgSrc = fileName;
                }
                if (ModelState.IsValid)
                {
                    db.Books.Add(book);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", book.LanguageId);
                ViewBag.WriterId = new SelectList(db.Writers, "Id", "Name", book.WriterId);
                return View(book);
            }
            else
            {
                return HttpNotFound();
            }

        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["admin"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Book book = db.Books.Find(id);
                if (book == null)
                {
                    return HttpNotFound();
                }
                var sounds = db.Sounds.ToList().Where(a => a.BookId == book.Id);
                ViewBag.Sounds = sounds;
                ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", book.LanguageId);
                ViewBag.WriterId = new SelectList(db.Writers, "Id", "Name", book.WriterId);
                return View(book);
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Info,ImgSrc,Price,DownloadCount,FavoriteCount,WriterId,LanguageId,Status")] Book book,string oldfile, string removeSounds, IEnumerable<HttpPostedFileBase> files)
        {
            if (Session["admin"] != null)
            {
                

                if (removeSounds.Length != 0)
                {
                    var removeSoundList = removeSounds.Split(';');

                    foreach (var item in removeSoundList)
                    {
                        var sound = db.Sounds.Find(Convert.ToInt32(item));
                        db.Sounds.Remove(sound);
                        System.IO.File.Delete(Server.MapPath("~/Uploads/Books/mp3/") + sound.SoundSource);
                    }
                }
                var Sound = db.Sounds.Where(x => x.Book.Id == book.Id).ToList().Last();


                int i = Sound.row + 1; 
                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string path = Guid.NewGuid() + Path.GetExtension(file.FileName);

                        Sound sound = new Sound();
                        sound.SoundSource = path;
                        sound.Book = book;
                        sound.BookId = book.Id;
                        sound.name = book.Title + "-" + i;
                        sound.row = i; i++;
                        db.Sounds.Add(sound);

                        file.SaveAs(Path.Combine(Server.MapPath("~/Uploads/Books/mp3/"), path));
                    }
                }

                var inputPhoto = HttpContext.Request.Files["ImgSrc"];
                if (inputPhoto.FileName.Length > 0)
                {
                    if (inputPhoto.ContentType == "image/gif" || inputPhoto.ContentType == "image/png" || inputPhoto.ContentType == "image/jpeg")
                    {
                        string oldPath = Request.MapPath("~/Uploads/Books/img/" + oldfile.Trim());
                        System.IO.File.Delete(oldPath);
                        string fileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff-") + inputPhoto.FileName;
                        string newFile = Path.Combine(HttpContext.Server.MapPath("~/Uploads/Books/img/"), fileName);
                        inputPhoto.SaveAs(newFile);
                        book.ImgSrc = fileName;
                    }
                }
                else
                {
                    book.ImgSrc = oldfile;
                }
                if (ModelState.IsValid)
                {
                    db.Entry(book).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", book.LanguageId);
                ViewBag.WriterId = new SelectList(db.Writers, "Id", "Name", book.WriterId);
                return View(book);
            }
            else
            {
                return HttpNotFound();
            }

           
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["admin"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Book book = db.Books.Find(id);
                if (book == null)
                {
                    return HttpNotFound();
                }
                return View(book);
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["admin"] != null)
            {
                Book book = db.Books.Find(id);
                if (book.ImgSrc != null)
                {
                    System.IO.File.Delete(Request.MapPath("~/Uploads/Books/img/" + book.ImgSrc));
                }

                var SoundsList = db.Sounds.ToList().Where(a=> a.BookId == book.Id);

                foreach (var item in SoundsList)
                {
                    System.IO.File.Delete(Request.MapPath("~/Uploads/Books/mp3/" + item.SoundSource));
                }

                db.Books.Remove(book);
                db.SaveChanges();
                return RedirectToAction("Index");
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
