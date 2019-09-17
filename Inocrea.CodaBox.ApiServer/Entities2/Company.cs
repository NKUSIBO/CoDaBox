using System;
using System.Collections.Generic;

namespace Inocrea.CodaBox.ApiServer.Entities2
{
    public partial class Company
    {
        public Company()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
            Order = new HashSet<Order>();
        }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Tva { get; set; }

        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
