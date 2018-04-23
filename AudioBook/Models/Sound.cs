using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AudioBook.Models
{
    public class Sound
    {
        public int Id { get; set; }
        public string name { get; set; }
        public String SoundSource { get; set; }
        public int row { get; set; }
        public int BookId { get; set; }


        //Maping 
        public virtual Book Book { get; set; }
        //

    }
}