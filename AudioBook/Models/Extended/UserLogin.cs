using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AudioBook.Models
{
    public class UserLogin
    {
        [Display(Name ="Email or Username")]
        [Required(AllowEmptyStrings =false, ErrorMessage = "Email or Username required")]
        public string EmailOrUsername { get; set; }

        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        public string password { get; set; }

        [Display(Name ="Remender me")]
        public bool RememberMe { get; set; }    
    }
}