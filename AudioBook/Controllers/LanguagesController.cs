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
    public class LanguagesController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Languages
        public ActionResult Index()
        {
            if (Session["admin"] != null)
            {
                return View(db.Languages.ToList());
            }
            else
            {
                return HttpNotFound();
            }
 
        }

        // GET: Languages/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["admin"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Language language = db.Languages.Find(id);
                if (language == null)
                {
                    return HttpNotFound();
                }
                return View(language);
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        // GET: Languages/Create
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

        // POST: Languages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Language language)
        {
            if (Session["admin"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Languages.Add(language);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(language);
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        // GET: Languages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["admin"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Language language = db.Languages.Find(id);
                if (language == null)
                {
                    return HttpNotFound();
                }
                return View(language);
            }
            else
            {
                return HttpNotFound();
            }
           
        }

        // POST: Languages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Language language)
        {
            if (Session["admin"] != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(language).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(language);
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        // GET: Languages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["admin"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Language language = db.Languages.Find(id);
                if (language == null)
                {
                    return HttpNotFound();
                }
                return View(language);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: Languages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["admin"] != null)
            {
                Language language = db.Languages.Find(id);
                db.Languages.Remove(language);
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
