using AudioBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace AudioBook.Controllers
{

    public class AudioBookController : Controller
    {
        private ModelContext db = new ModelContext();

       
        // GET: AudioBook

        public ActionResult Index()
        {

            ViewBag.Books = (from a in db.Books
                             join c in db.Writers on a.Writer equals c
                             select a
                          ).ToList().OrderByDescending(p => p.Id).Take(12);


            ViewBag.PopularWriters = db.PlayaLists
                                    .Join(db.Books, p => p.BookId, pc => pc.Id, (p, pc) => new { p, pc })
                                    .GroupBy(d => d.pc).OrderByDescending(gp => gp.Count()).Select(g => g.Key)
                                    .Join(db.Writers, b => b.WriterId, w => w.Id, (b, w) => new { b, w }).Select(q => q.w);

            ViewBag.BestSeller = db.Books.OrderByDescending(m => m.DownloadCount).ToList()
                                    .Join(db.Writers, m => m.WriterId, w => w.Id, (m, w) => new { m, w })
                                    .Select(t => t.m).Take(5);
            ViewBag.TopWriters = ViewBag.BestSeller;

            ViewBag.RecomendedBooks = db.Books.OrderByDescending(m => m.FavoriteCount).ToList()
                                    .Join(db.Writers, m => m.WriterId, w => w.Id, (m, w) => new { m, w })
                                    .Select(t => t.m).Take(5);

            ViewBag.TopGenres = db.BookGenres.Select(l => new { l.BookId, l.GenreId }).Distinct().GroupBy(g => g.GenreId).OrderByDescending(bg => bg.Count()).ToList()
                                .Join(db.Genres, bg => bg.Key, g => g.Id, (bg, g) => new { bg, g }).Select(t => t.g).Take(6);


            return View();
        }
        public ActionResult BookDetail(int? id)
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

            var Genres = db.BookGenres.Where(a => a.BookId == book.Id).ToList();
            string s = "";
            for (int i = 0; i < Genres.Count; i++)
            {
                if (i == Genres.Count - 1)
                    s += Genres[i].Genre.Name;
                else
                    s += Genres[i].Genre.Name + " , ";
            }
            ViewBag.Genre = s;
            ViewBag.TopBooks = ViewBag.BestSeller = db.Books.OrderByDescending(m => m.DownloadCount).ToList()
                                    .Join(db.Writers, m => m.WriterId, w => w.Id, (m, w) => new { m, w })
                                    .Select(t => t.m).Take(5);
            ViewBag.RelatedBooks = db.Books.Where(p => p.Writer.Id == book.Writer.Id && p.Id != book.Id).ToList();

            List<Sound> SongsList = db.Sounds.Where(c => c.BookId == id).ToList();

            var listS = new List<Sound>();
            foreach (var item in SongsList)
            {
                var sound = new Sound()
                {
                    Id = item.Id,
                    BookId = item.BookId,
                    name = item.name,
                    SoundSource = item.SoundSource,
                    row = item.row
                };
                listS.Add(sound);
            }

            ViewBag.StartSound = listS[0].SoundSource;

            string json = JsonConvert.SerializeObject(listS);

            ViewBag.SoundsList = json;

            return View(book);


        }

        //[HttpPost]
        //public JsonResult BookSounds(int? id)
        //{
            
        //    return Json(json);
        //}

        [HttpGet]
        public ActionResult Genres(int page = 1, int pageSize = 12)
        {
            
            ViewBag.AllGenres = db.Genres.OrderBy(p=> p.Name).ToList();

            var AllBooksList = db.Books.ToList();

            PagedList<Book> AllBooks = new PagedList<Book>(AllBooksList, page, pageSize);

            return View(AllBooks);
        }
        [HttpGet]
        public ActionResult UserProfilView(int? id, int page = 1, int pageSize = 12)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Genre genre = db.Genres.Find(id);
            User user = db.Users.Find(id);
            
            if (user == null)
            {
                return HttpNotFound();
            }
            
            List<Book> PlaylistBooks = db.PlayaLists.Where(x => x.UserId == user.Id)
                .Select(c => c.Book)
                .ToList();

            PagedList<Book> Books = new PagedList<Book>(PlaylistBooks, page, pageSize);


            return View(Books);
        }

        [HttpGet]
        public ActionResult Genre(int? id,int page = 1, int pageSize = 12)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Genre genre = db.Genres.Find(id);

            if(genre == null)
            {
                return HttpNotFound();
            }

            ViewBag.AllGenres = db.Genres.OrderBy(p => p.Name).ToList();
            ViewBag.Genre = genre;

            List<Book> BooksList = db.BookGenres.Where(x => x.GenreId == genre.Id)
                .Select(c => c.Book)
                .ToList();

            PagedList<Book> Books = new PagedList<Book>(BooksList, page, pageSize);
            

            return View(Books);
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult User()
        {
            return View();
        }

        public ActionResult Writer(int page = 1, int pageSize = 12)
        {
            ViewBag.TopWriters = db.Books.OrderByDescending(m => m.DownloadCount).Select(q => q)
                                    .Join(db.Writers, m => m.WriterId, w => w.Id, (m, w) => new { m, w })
                                    .Select(t => t.w).Take(5);
            var AllWriterList = db.Writers.ToList();

            PagedList<Writer> AllWriters = new PagedList<Writer>(AllWriterList, page, pageSize);



            return View(AllWriters);
        }

        public ActionResult WriterDetail(int? id, int page = 1, int pageSize = 6)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var writer = db.Writers.Find(id);
            if (writer == null)
            {
                return HttpNotFound();
            }
            ViewBag.writer = writer;
            ViewBag.TopWriters = db.Books.OrderByDescending(m => m.DownloadCount).Select(q => q)
                                    .Join(db.Writers, m => m.WriterId, w => w.Id, (m, w) => new { m, w })
                                    .Select(t => t.w).Take(5);

            List<Book> WriterBooks = db.Books.Where(p => p.WriterId == id).ToList();

            PagedList<Book> WriterAllBooks = new PagedList<Book>(WriterBooks, page, pageSize);


            return View(WriterAllBooks);
        }


        public ActionResult test()
        {


            return View();
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

