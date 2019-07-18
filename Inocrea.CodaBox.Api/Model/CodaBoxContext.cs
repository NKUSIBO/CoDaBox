using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Inocrea.CodaBox.Api.Model
{
    
        public class CodaBoxContext : DbContext
        {
            public CodaBoxContext(DbContextOptions<CodaBoxContext> options)
                : base(options)
            {
            }

            
        }
    
}
