using System;
using System.Collections.Generic;

namespace Inocrea.CodaBox.ApiServer.Entities
{
    public partial class Statements
    {
        public Statements()
        {
            Transactions = new HashSet<Transactions>();
        }

        public int StatementId { get; set; }
        public int CompteBancaireId { get; set; }
        public double InitialBalance { get; set; }
        public double NewBalance { get; set; }
        public string InformationalMessage { get; set; }
        public DateTime Date { get; set; }

        public virtual CompteBancaire CompteBancaire { get; set; }
        public virtual ICollection<Transactions> Transactions { get; set; }
    }
}
