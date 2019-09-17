using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inocrea.CodaBox.ApiServer.Entities2.ViewModel
{
    public class TransactionsAccountViewModel
    {
       
        public string StructuredMessage { get; set; }
        public DateTime TransactionDate { get; set; }
        
       
        public DateTime ValueDate { get; set; }
        public string Date
        {
            get
            {
                return ValueDate.ToString("dd/MM/yyyy");
            }
        }
        [Display(Name = "Montant")]
        public double Amount { get; set; }
        public int Id { get; set; }
        [Display(Name = "Description")]
        public string Message { get; set; }

        [Display(Name = "CompteBancaire")]
        public string Iban { get; set; }
        public string CurrencyCode { get; set; }
        public string Bic { get; set; }
        public string IdentificationNumber { get; set; }
      
    }
}
