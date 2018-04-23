using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace AudioBook.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [DataType(DataType.Text)]
        public string Info { get; set; }
        [DataType(DataType.ImageUrl)]
        public string ImgSrc { get; set; }
        public int Price { get; set; }
        public int DownloadCount  { get; set; }
        public int FavoriteCount { get; set; }
        public int WriterId { get; set; }
        public int LanguageId { get; set; }   
        public bool Status { get; set; }


        //Maping
        public virtual Writer Writer { get; set; }
        public virtual Language Language { get; set; }

        //
        public virtual List<PlayList> PlayList { get; set; }
        public virtual List<BookGenre> BookGenre { get; set; }
        public virtual List<Sound> Sound { get; set; }


    }
}