using System;
using System.Collections.Generic;

namespace Inocrea.CodaBox.Web.Entities
{
    public partial class Transactions
    {
        public int AdresseId { get; set; }
        public string StructuredMessage { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime ValueDate { get; set; }
        public decimal Amount { get; set; }
        public int Id { get; set; }
        public int StatementId { get; set; }
        public int CompteBancaireId { get; set; }
        public string  Message { get; set; }
        public CompteBancaire CompteBancaire { get; set; }
        public Statements Statement { get; set; }
    }
}
