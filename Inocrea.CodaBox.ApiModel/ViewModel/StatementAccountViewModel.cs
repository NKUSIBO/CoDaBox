using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Inocrea.CodaBox.ApiModel.ViewModel
{
    public class StatementAccountViewModel
    {
        [DataMember]
        public decimal InitialBalance { get; set; }
        [DataMember]
        public decimal NewBalance { get; set; }
        [DataMember]
        public string InformationalMessage { get; set; }
        [DataMember]

        public DateTime Date { get; set; }
        [DataMember]
        public int StatementId { get; set; }
        [DataMember]
        public string Iban { get; set; }
        [DataMember]
        public string CurrencyCode { get; set; }
        [DataMember]
        public string Bic { get; set; }
        [DataMember]
        public string IdentificationNumber { get; set; }
    }
}
