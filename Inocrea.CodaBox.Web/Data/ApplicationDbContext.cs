using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Inocrea.CodaBox.ApiModel;

namespace Inocrea.CodaBox.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Inocrea.CodaBox.ApiModel.InvoiceModel> InvoiceModel { get; set; }
    }
}
