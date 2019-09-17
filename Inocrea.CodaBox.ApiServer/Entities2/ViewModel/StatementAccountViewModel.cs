using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using Inocrea.CodaBox.ApiModel.Models;

namespace Inocrea.CodaBox.ApiServer.Entities2.ViewModel
{
    public class StatementAccountViewModel
    {
       

        [DataMember]
        public double InitialBalance { get; set; }
        [DataMember]
        public double NewBalance { get; set; }
        [DataMember]
        public string InformationalMessage { get; set; }
        [DataMember]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy}")]
        public DateTime Date { get; set; }
        public string DateFrmt
        {
            get
            {
                return Date.ToString("dd/MM/yyyy");
            }
        }
        [Key]
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
    //public class StatementAccountViewModel
    //{
    //    [DataMember]
    //    public CompteBancaire Compte { get; set; }
    //    [DataMember]
    //    public Statements Stat { get; set; }
    //}
}
