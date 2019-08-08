using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Identity.Entities
{
   
        public class ApplicationUser : IdentityUser
        {
           
            [MaxLength(200)]
            public String FirstName { get; set; }

           
            [MaxLength(250)]
            public String LastName { get; set; }



        }
    
}
