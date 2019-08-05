using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inocrea.CodaBox.ApiServer.Entities
{
    public class Statements
    {
        public Statements() => Transactions = new HashSet<Transactions>();

        [Key]
        public int StatementId { get; set; }
        [ForeignKey("CompteBancaire")]
        public int CompteBancaireId { get; set; }

        public double InitialBalance { get; set; }
        public double NewBalance { get; set; }
        public string InformationalMessage { get; set; }
        public DateTime Date { get; set; }

        public virtual CompteBancaire CompteBancaire { get; set; }
        public virtual ICollection<Transactions> Transactions { get; set; }
    }
}