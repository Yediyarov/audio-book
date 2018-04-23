using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AudioBook.Models
{
    public class PlayList
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }

        //Maping
        public virtual User User { get; set; }
        public virtual Book Book { get; set; }
    }
}