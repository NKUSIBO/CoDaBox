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
        public String AccountingDate { get; set; }
        [DataMember]
        public DateTime ValueDate { get; set; }
        [DataMember]
        public string CounterParty { get; set; }
        [DataMember]
        public string TransactionMessage { get; set; }
        [DataMember]
        public decimal Balance { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
    }
}
