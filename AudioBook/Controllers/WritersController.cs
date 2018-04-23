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

namespace AudioBook.Controllers
{
    public class WritersController : Controller
    {
        private ModelContext db = new ModelContext();

        // GET: Writers
        public ActionResult Index()
        {
            if (Session["admin"] != null)
            {
                return View(db.Writers.ToList());
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        // GET: Writers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Writer writer = db.Writers.Find(id);
            if (writer == null)
            {
                return HttpNotFound();
            }
            return View(writer);
        }

        // GET: Writers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Writers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Surname,Info,ImgSrc,Status")] Writer writer)
        {
            var inputPhoto = HttpContext.Request.Files["ImgSrc"];

            if (inputPhoto.ContentType=="image/gif" || inputPhoto.ContentType == "image/png" || inputPhoto.ContentType == "image/jpeg" || inputPhoto.ContentType == "image/jpg")
            {
                string fileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff-")+inputPhoto.FileName;
                string newFile = Path.Combine(HttpContext.Server.MapPath("~/Uploads/Writers/img/"),fileName);
                inputPhoto.SaveAs(newFile);
                writer.ImgSrc = fileName;
            }

            if (ModelState.IsValid)
            {
                db.Writers.Add(writer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(writer);
        }

        // GET: Writers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Writer writer = db.Writers.Find(id);
            if (writer == null)
            {
                return HttpNotFound();
            }
            return View(writer);
        }

        // POST: Writers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Surname,Info,ImgSrc,Status")] Writer writer, string oldFile)
        {
            var inputPhoto = HttpContext.Request.Files["ImgSrc"];
            if(inputPhoto.FileName.Length > 0)
            {
                if (inputPhoto.ContentType == "image/gif" || inputPhoto.ContentType == "image/png" || inputPhoto.ContentType == "image/jpeg")
                {
                    if (oldFile.Length > 0)
                    {
                        string oldPath = Request.MapPath("~/Uploads/Writers/img/" + oldFile.Trim());
                        System.IO.File.Delete(oldPath);
                    }
                    string fileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff-") + inputPhoto.FileName;
                    string newFile = Path.Combine(HttpContext.Server.MapPath("~/Uploads/Writers/img/"), fileName);
                    inputPhoto.SaveAs(newFile);
                    writer.ImgSrc = fileName;
                }
            }
            else
            {
                writer.ImgSrc = oldFile;
            }
            if (ModelState.IsValid)
            {
                db.Entry(writer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(writer);
        }

        // GET: Writers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Writer writer = db.Writers.Find(id);
            if (writer == null)
            {
                return HttpNotFound();
            }
            return View(writer);
        }

        // POST: Writers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Writer writer = db.Writers.Find(id);
            if (writer.ImgSrc != null)
            {
                System.IO.File.Delete(Request.MapPath("~/Uploads/Writers/img/" + writer.ImgSrc));
            }
            db.Writers.Remove(writer);
            db.SaveChanges();
            return RedirectToAction("Index");
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
