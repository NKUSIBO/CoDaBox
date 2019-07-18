using System;
using System.Collections.Generic;

namespace Inocrea.CodaBox.Dal.Models
{
    public partial class Country
    {
        public Country()
        {
            Adress = new HashSet<Adress>();
        }

        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public ICollection<Adress> Adress { get; set; }
    }
}
