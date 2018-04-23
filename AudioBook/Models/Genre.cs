using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AudioBook.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }

        //Maping

        //
        public virtual List<BookGenre> BookGenre { get; set; }
    }
}