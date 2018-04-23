using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AudioBook.Models
{
    public class BookGenre
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int GenreId { get; set; }

        //Maping
        public virtual Book Book { get; set; }
        public virtual Genre Genre { get; set; }
    }
}