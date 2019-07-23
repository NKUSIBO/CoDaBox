using System;
using System.Collections.Generic;

namespace Inocrea.CodaBox.Dal.Models
{
    public partial class Company
    {
        public Company()
        {
           
        }

        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Tva { get; set; }

      
      
    }
}
