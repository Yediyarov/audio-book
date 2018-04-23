using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AudioBook.Models;

namespace AudioBook.Controllers
{
    public class BookGenresController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: BookGenres
        public ActionResult Index()
        {
            if (Session["admin"] != null)
            {
                var bookGenres = db.BookGenres.Include(b => b.Book).Include(b => b.Genre);
                return View(bookGenres.ToList());
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        // GET: BookGenres/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["admin"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                BookGenre bookGenre = db.BookGenres.Find(id);
                if (bookGenre == null)
                {
                    return HttpNotFound();
                }
                return View(bookGenre);
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        // GET: BookGenres/Create
        public ActionResult Create()
        {
            if (Session["admin"] != null)
            {
                ViewBag.BookId = new SelectList(db.Books, "Id", "Title");
                ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name");
                return View();
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        // POST: BookGenres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BookId,GenreId")] BookGenre bookGenre)
        {
            if (ModelState.IsValid)
            {
                db.BookGenres.Add(bookGenre);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookId = new SelectList(db.Books, "Id", "Title", bookGenre.BookId);
            ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name", bookGenre.GenreId);
            return View(bookGenre);
        }

        // GET: BookGenres/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["admin"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                BookGenre bookGenre = db.BookGenres.Find(id);
                if (bookGenre == null)
                {
                    return HttpNotFound();
                }
                ViewBag.BookId = new SelectList(db.Books, "Id", "Title", bookGenre.BookId);
                ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name", bookGenre.GenreId);
                return View(bookGenre);
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        // POST: BookGenres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BookId,GenreId")] BookGenre bookGenre)
        {
            if (Session["admin"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(bookGenre).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.BookId = new SelectList(db.Books, "Id", "Title", bookGenre.BookId);
                ViewBag.GenreId = new SelectList(db.Genres, "Id", "Name", bookGenre.GenreId);
                return View(bookGenre);
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        // GET: BookGenres/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["admin"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                BookGenre bookGenre = db.BookGenres.Find(id);
                if (bookGenre == null)
                {
                    return HttpNotFound();
                }
                return View(bookGenre);
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        // POST: BookGenres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["admin"] != null)
            {
                BookGenre bookGenre = db.BookGenres.Find(id);
                db.BookGenres.Remove(bookGenre);
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
