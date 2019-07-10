using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Inocrea.CodaBox.ApiModel
{
    public class Transactions
    {
        [Key]
        public int Id { get; set; }
        [DataMember]
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
