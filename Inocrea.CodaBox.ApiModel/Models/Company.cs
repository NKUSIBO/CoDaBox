using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inocrea.CodaBox.ApiModel.Models
{
    public  class Company
    {
        public Company()
        {
        }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Tva { get; set; }

        public virtual ICollection<CompteBancaire> CompteBancaire { get; set; }
}
}
