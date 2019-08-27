using System;
using System.Collections.Generic;

namespace Inocrea.CodaBox.Web.Entities
{
    public partial class CompteBancaire
    {
        public CompteBancaire()
        {
            Statements = new HashSet<Statements>();
            Transactions = new HashSet<Transactions>();
        }

        public int Id { get; set; }
        public string Iban { get; set; }
        public string Bic { get; set; }
        public string IdentificationNumber { get; set; }
        public string CurrencyCode { get; set; }
        public string OwnerTva { get; set; }

        public Company OwnerTvaNavigation { get; set; }
        public ICollection<Statements> Statements { get; set; }
        public ICollection<Transactions> Transactions { get; set; }
    }
}
