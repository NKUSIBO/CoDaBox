using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebCodaBox.Models
{
    public class CodaBoxContext:IdentityDbContext<CodaBoxUser,CodaBoxRole,int>
    {
        public CodaBoxContext(DbContextOptions<CodaBoxContext>options):base (options)
        {
                
        }
    }
}
