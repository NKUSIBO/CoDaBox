﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inocrea.CodaBox.ApiServer.Entities
{
    [Table("SepaDirectDebits")]
    public class SepaDirectDebit
    {
        [Key]
        public int SepaDirectDebitId { get; set; }

        public string CreditorIdentificationCode { get; set; }
        public string MandateReference { get; set; }
        public int PaidReason { get; set; }
        public int Scheme { get; set; }
        public int Type { get; set; }

        [ForeignKey("Transactions")]
        public int TransactionId { get; set; }

        public virtual Transactions Transactions { get; set; }

        public override string ToString()
        {
            return CreditorIdentificationCode+' '+MandateReference+" Paid reason"+PaidReason ;
        }
    }
}