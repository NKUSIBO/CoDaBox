using System;
using System.Collections.Generic;

namespace Inocrea.CodaBox.ApiServer.Entities2
{
    public partial class Company
    {
        public Company()
        {
            AspNetUsers = new HashSet<AspNetUsers>();
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

        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
        public virtual ICollection<CompteBancaire> CompteBancaire { get; set; }
    }
}
