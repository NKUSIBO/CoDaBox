using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Inocrea.CodaBox.ApiServer.Entities
{
    public partial class Transactions
    {
        public int Id { get; set; }
        [JsonIgnore]
        public int StatementId { get; set; }
        [JsonIgnore]
        public int CompteBancaireId { get; set; }
        public string StructuredMessage { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime ValueDate { get; set; }
        public double Amount { get; set; }
        public string Message { get; set; }
        public int? AdresseId { get; set; }

        public virtual CompteBancaire CompteBancaire { get; set; }
        [JsonIgnore]
        public virtual Statements Statement { get; set; }
    }
}
