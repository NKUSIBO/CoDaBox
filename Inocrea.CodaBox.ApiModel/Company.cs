﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Inocrea.CodaBox.ApiModel
{
    public partial class Company
    {
        public Company()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
            CompteBancaire = new HashSet<CompteBancaire>();
        }

        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Tva { get; set; }

        public Adress Adress { get; set; }
        public ICollection<AspNetUsers> AspNetUsers { get; set; }
        public ICollection<CompteBancaire> CompteBancaire { get; set; }
    }

}
