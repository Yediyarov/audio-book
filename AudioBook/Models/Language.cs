using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AudioBook.Models
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Maping
        
        //
        public virtual List<Book> Book { get; set; }
    }
}