using System;
using System.Collections.Generic;
using System.Text;

namespace Inocrea.CodaBox.ApiModel.ViewModel
{
    public class TransactionsAccountViewModel
    {
        public string StructuredMessage { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime ValueDate { get; set; }
        public double Amount { get; set; }
        public int Id { get; set; }
        public string Message { get; set; }
        public string Iban { get; set; }
        public string CurrencyCode { get; set; }
        public string Bic { get; set; }
        public string IdentificationNumber { get; set; }
        public int? StatementsId { get; set; }
        public int? TransactionsId { get; set; }
    }
}
