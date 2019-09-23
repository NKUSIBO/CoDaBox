﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Inocrea.CodaBox.ApiModel.Models;
using Microsoft.AspNetCore.Identity;

namespace WebCodaBox.Models
{
    public class CodaBoxUser:IdentityUser<int>
    {
        [MaxLength(200)]
        public string FirstName { get; set; }


        [MaxLength(250)]
        public string LastName { get; set; }
        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }
    }
}
