using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace AudioBook.Models
{
    public class Writer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Info { get; set; }
        [DataType(DataType.ImageUrl)]
        public string ImgSrc { get; set; }
        public bool Status { get; set; }

        //Maping
        public virtual List<Book> Book { get; set; }
    }
}