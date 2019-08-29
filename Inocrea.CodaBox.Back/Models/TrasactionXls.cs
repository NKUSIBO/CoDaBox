using System;
using Inocrea.CodaBox.ApiModel.Models;

namespace Inocrea.CodaBox.ApiServer.Models
{
    public class TrasactionXls
    {
        public DateTime Date { get; set; }
        public double Montant { get; set; }
        public string Description { get; set; }
        public string Bénéficiaire { get; set; }

        public TrasactionXls(Transactions transactions)
        {
            Date = transactions.ValueDate;
            Montant = transactions.Amount;
            Description = transactions.StructuredMessage;
            Bénéficiaire = transactions.CompteBancaire.Iban;
        }
    }
}
