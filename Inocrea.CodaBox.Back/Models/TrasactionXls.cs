using System;
using System.Linq;

using Inocrea.CodaBox.ApiModel.Models;
using Inocrea.CodaBox.Back.Entities;

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

        private void GetDescription(Transactions transactions)
        {
            //nom du client + numero de facture (communication structuré)
            var db = new InosysDBContext();
            var co = db.CompteBancaire.Where(cb => cb.Id == transactions.CompteBancaireId).First();
            var iban = co.Iban;
            var com = transactions.StructuredMessage;
        }
    }
}
