using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Inocrea.CodaBox.ApiModel;

namespace Inocrea.CodaBox.ApiModel
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

        public CompteBancaire CompteBancaire { get; set; }
        public ICollection<Transactions> Transactions { get; set; }
    }
}
