using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Type Username!")]
        public string UserName { set; get; }
        
        [Required(ErrorMessage = "Type PassWord!")]
        public string PassWord { set; get; }
        
        public bool RememberMe { set; get; }
    }
}