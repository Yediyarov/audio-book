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
    public class GenresController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Genres
        public ActionResult Index()
        {
            if (Session["admin"] != null)
            {
                return View(db.Genres.ToList());
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        // GET: Genres/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["admin"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Genre genre = db.Genres.Find(id);
                if (genre == null)
                {
                    return HttpNotFound();
                }
                return View(genre);
            }
            else
            {
                return HttpNotFound();
            }
           
        }

        // GET: Genres/Create
        public ActionResult Create()
        {
            if (Session["admin"] != null)
            {
                return View();
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: Genres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Status")] Genre genre)
        {
            if (Session["admin"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Genres.Add(genre);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(genre);
            }
            else
            {
                return HttpNotFound();
            }

        }

        // GET: Genres/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["admin"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Genre genre = db.Genres.Find(id);
                if (genre == null)
                {
                    return HttpNotFound();
                }
                return View(genre);
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Status")] Genre genre)
        {
            if (Session["admin"] != null)
            {
                if (ModelState.IsValid)
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(genre).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(genre);
                }

                return View(genre);
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        // GET: Genres/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["admin"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Genre genre = db.Genres.Find(id);
                if (genre == null)
                {
                    return HttpNotFound();
                }
                return View(genre);
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["admin"] != null)
            {
                Genre genre = db.Genres.Find(id);
                db.Genres.Remove(genre);
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
