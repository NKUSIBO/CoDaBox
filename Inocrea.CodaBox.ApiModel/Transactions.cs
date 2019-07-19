using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text;

namespace Inocrea.CodaBox.ApiModel
{
    
    public class Transactions
    {
        [Key]
        public int Id { get; set; }
        [DataMember]
        
        public string AccountingDate { get; set; }

        [DataMember]
        public string Bic { get; set; }
        //[DataMember]
        //public string CounterParty { get; set; }


       
        [DataMember]
        public decimal NewBalance { get; set; }
        [DataMember]
        public decimal InitialBalance { get; set; }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string CurrencyCode { get; set; }
        [DataMember]
        public string NumeroIdentification { get; set; }
        [DataMember]
        public string Number { get; set; }
        [DataMember]
        public string Amount { get; set; }
        [DataMember]
        public string TransactionDate { get; set; }

        [DataMember]
        public DateTime ValueDate { get; set; }
        [DataMember]
        public string StructuredMessage { get; set; }
        [DataMember]
        public string Message { get; set; }

        public int AdresseId { get; set; }
     
        public int StatementId { get; set; }
        public int CompteBancaireId { get; set; }

        public CompteBancaire CompteBancaire { get; set; }
        public Statements Statement { get; set; }
    }
}
