using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inocrea.CodaBox.ApiModel.Models
{
    public class LoginModel
    {
        [Required]
        public String Username { get; set; }

        [Required]
   
        public String Password { get; set; }

        public  string Token { get; set; }

        public DateTime Expiration { get; set; }
    }
}
