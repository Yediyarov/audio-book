using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AudioBook.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.ImageUrl)]
        public string ImgSrc { get; set; }
        public bool Status { get; set; }
        public System.Guid ActivationCode { get; set; }
        public int RoleId { get; set; }

        //Maping
        
        //
        public virtual List<PlayList> PlayList { get; set; }

    }
}