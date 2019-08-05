using System;
using System.Collections.Generic;
using System.Text;

namespace Inocrea.CodaBox.ApiModel.ViewModel
{
    public class TransactionsAccountViewModel
    {
        public string StructuredMessage { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionDateFrmt
        {
            get
            {
                return TransactionDate.ToString("dd/MM/yyyy");
            }
        }
        public DateTime ValueDate { get; set; }
        public string ValueDateFrmt
        {
            get
            {
                return ValueDate.ToString("dd/MM/yyyy");
            }
        }
        public double Amount { get; set; }
        public int Id { get; set; }
        public string Message { get; set; }
        public string Iban { get; set; }
        public string CurrencyCode { get; set; }
        public string Bic { get; set; }
        public string IdentificationNumber { get; set; }
      
    }
}
