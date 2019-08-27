using System;
using System.Collections.Generic;

namespace Inocrea.CodaBox.ApiServer.Entities2
{
    public partial class CompteBancaire
    {
        public CompteBancaire()
        {
            Statements = new HashSet<Statements>();
            Transactions = new HashSet<Transactions>();
        }

        public int CompteBancaireId { get; set; }
        public string Iban { get; set; }
        public string Bic { get; set; }
        public string IdentificationNumber { get; set; }
        public string CurrencyCode { get; set; }
        public int? CompanyId { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<Statements> Statements { get; set; }
        public virtual ICollection<Transactions> Transactions { get; set; }
    }
}
