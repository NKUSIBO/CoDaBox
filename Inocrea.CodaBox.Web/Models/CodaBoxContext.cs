using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebCodaBox.Models;

namespace Inocrea.CodaBox.Web.Models
{
    public class CodaBoxContext:IdentityDbContext<CodaBoxUser>
    {
        public CodaBoxContext(DbContextOptions<CodaBoxContext>options):base (options)
        {
                
        }
    }
}
