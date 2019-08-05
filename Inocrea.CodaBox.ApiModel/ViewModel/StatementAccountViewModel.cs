using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy}")]
        public DateTime Date { get; set; }
        public string Date_Frmt
        {
            get
            {
                return Date.ToString("dd/MM/yyyy");
            }
        }

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
        [DataMember]
        public List<Transactions> Transactions { get; set; }
    }
}
