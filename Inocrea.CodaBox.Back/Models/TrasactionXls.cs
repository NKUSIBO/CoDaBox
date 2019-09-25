using System;
using Inocrea.CodaBox.ApiModel.Models;

namespace Inocrea.CodaBox.Back.Models
{
    public class TrasactionXls
    {
        public DateTime Date { get; set; }
        public double Montant { get; set; }
        public string Description { get; set; }
        public string Beneficiaire { get; set; }

        public TrasactionXls(Transactions transactions)
        {
            Date = transactions.ValueDate;
            Montant = transactions.Amount;
            Description = transactions.ContrePartie + " ";
            if (!string.IsNullOrEmpty(transactions.StructuredMessage))
                Description += Format(transactions.StructuredMessage);
            else
                Description += Format(transactions.Message);
            if (transactions.CompteBancaire.Iban != null)
                Beneficiaire = transactions.CompteBancaire.Iban;
        }

        private string Format(string txt)
        {
            txt = txt.Replace('\n', '|');
            txt = txt.Replace('\t', '|');
            return txt;
        }
    }
}
