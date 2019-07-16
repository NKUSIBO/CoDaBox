using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Inocrea.CodaBox.Web.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        [MaxLength(200)]
        public string CompanyVAT{ get; set; }

       
    }
}
