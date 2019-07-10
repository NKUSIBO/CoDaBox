using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Inocrea.CodaBox.ApiModel
{
    public class StatementsTransactionViewModel
    {
        [Key]
        public int Id { get; set; }
        public InvoiceModel Testaments { get; set; }
        public Transactions Transactions { get; set; }
    }
}
