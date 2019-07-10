using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Inocrea.CodaBox.ApiModel
{
    [DataContract]
    public class InvoiceModel
    {
        [Key]
        public int Id { get; set; }
        [DataMember]
        public string AccountingDate { get; set; }
      
        [DataMember]
        public string Bic { get; set; }
        [DataMember]
        public string CounterParty { get; set; }
       
       
       
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
        public ICollection<Transactions> Transactions { get; set; }
        public string Amount { get; set; }
        [DataMember]
        public string TransactionDate { get; set; }

        [DataMember]
        public string ValueDate { get; set; }
        [DataMember]
        public string StructuredMessage { get; set; }
        [DataMember]
        public string Message { get; set; }
    }
}
