using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Inocrea.CodaBox.ApiServer.Models
{
    public partial class Transactions
    {

        public Transactions( )
        {
        }
        public int AdresseId { get; set; }
        public string StructuredMessage { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime ValueDate { get; set; }
        public decimal Amount { get; set; }
        public int Id { get; set; }
        public int StatementId { get; set; }
        public int CompteBancaireId { get; set; }

        public virtual CompteBancaire CompteBancaire { get; set; }
        public virtual Statements Statement { get; set; }
    }
}
