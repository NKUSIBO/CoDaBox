using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Inocrea.CodaBox.ApiServer.Entities
{
    public class Transactions
    {
        [Key]
        public int Id { get; set; }
        public virtual CompteBancaire CompteBancaire { get; set; }

        [JsonIgnore]
        public virtual Statements Statement { get; set; }
        public string StructuredMessage { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime ValueDate { get; set; }
        public double Amount { get; set; }
        public string Message { get; set; }
        public int? AdresseId { get; set; }

        [ForeignKey("Statements")]
        public int StatementId { get; set; }
        [ForeignKey("CompteBancaire"), JsonIgnore]
        public int CompteBancaireId { get; set; }
    }
}