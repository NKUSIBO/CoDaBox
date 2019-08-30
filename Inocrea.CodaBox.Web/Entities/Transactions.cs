using System;
using System.Collections.Generic;

namespace Inocrea.CodaBox.Web.Entities
{
    public partial class Transactions
    {
        public int Id { get; set; }
        public string StructuredMessage { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime ValueDate { get; set; }
        public double Amount { get; set; }
        public string Message { get; set; }
        public int? AdresseId { get; set; }
        public int StatementId { get; set; }
        public int CompteBancaireId { get; set; }

        public CompteBancaire CompteBancaire { get; set; }
        public Statements Statement { get; set; }
        public SepaDirectDebits SepaDirectDebits { get; set; }
    }
}
