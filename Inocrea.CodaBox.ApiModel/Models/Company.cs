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

            CompteBancaire = new HashSet<CompteBancaire>();
        }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Tva { get; set; }
        public string NumeroClient { get; set; }
        public int FormJuridique { get; set; }
        public int AccountType { get; set; }
        public string ActivitSic { get; set; }
        public string Nentreprise { get; set; }

        public virtual ICollection<CompteBancaire> CompteBancaire { get; set; }
}
}
