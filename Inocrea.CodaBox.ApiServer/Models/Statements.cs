using System;
using System.Collections.Generic;

namespace Inocrea.CodaBox.ApiServer.Models
{
    public partial class Statements
    {
        public Statements()
        {
            Transactions = new HashSet<Transactions>();
        }

        public decimal InitialBalance { get; set; }
        public decimal NewBalance { get; set; }
        public string InformationalMessage { get; set; }
        public DateTime Date { get; set; }
        public int IdIban { get; set; }
        public int StatementId { get; set; }
        public int CompteBancaireId { get; set; }

        public virtual CompteBancaire CompteBancaire { get; set; }
        public virtual ICollection<Transactions> Transactions { get; set; }
    }
}
