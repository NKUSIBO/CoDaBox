using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


using Newtonsoft.Json;

namespace Inocrea.CodaBox.ApiModel.Models
{
    public class Transactions
    {
        [Key]
        public int Id { get; set; }






        [Display(Name = "Message")]
       
        public string StructuredMessage { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime ValueDate { get; set; }
        public double Amount { get; set; }
        public string Message { get; set; }
      

        [ForeignKey("Statements")]
        public int StatementId { get; set; }
        [ForeignKey("CompteBancaire")]
        public int CompteBancaireId { get; set; }

        [JsonIgnore]

        public virtual CompteBancaire CompteBancaire { get; set; }
        [JsonIgnore]
        public virtual Statements Statement { get; set; }
        public virtual SepaDirectDebit SepaDirectDebit { get; set; }

    }
}