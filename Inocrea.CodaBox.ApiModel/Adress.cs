using System;
using System.Collections.Generic;

namespace Inocrea.CodaBox.ApiModel
{
    public partial class Adress
    {
        public int AdressId { get; set; }
        public string AdressZipCode { get; set; }
        public bool AdressType { get; set; }
        public bool AdressActive { get; set; }
        public int CountryId { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public int CompanyId { get; set; }

        public Company AdressNavigation { get; set; }
        public Country Country { get; set; }
    }
}
