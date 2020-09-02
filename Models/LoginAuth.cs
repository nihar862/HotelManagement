using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotel_Management.Models
{
    public class LoginAuth
    {
        
        public int Id { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter UserName")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        public string Password { get; set; }
    }
}