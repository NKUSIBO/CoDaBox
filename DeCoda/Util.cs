using System;
using System.Collections.Generic;
using Inocrea.CodaBox.ApiModel.Models;

namespace DeCoda
{
    public class Util
    {
        public static DateTime GetDate(string date)
        {
            var day = int.Parse(date.Substring(0, 2));
            var mon = int.Parse(date.Substring(2, 2));
            var yer = int.Parse(date.Substring(4, 2));
            yer += 2000;
            return new DateTime(yer, mon, day);
        }


        public static Statements GetStatement(Record record)
        {
            var cb = new CompteBancaire
            {
                Bic = record.header.BIC,
                Iban = record.newSolde.NumCompte,
                //Titulaire = newSolde.NomTitulaire,
                CurrencyCode = record.newSolde.Devise,
            };

            var trs = new List<Transactions>();
            foreach (var mv in record.mouvements)
            {
                var tr = GetTransaction(mv);
                if (tr != null)
                    trs.Add(tr);
            }


            var st = new Statements
            {
                CompteBancaire = cb,
                Date = Util.GetDate(record.header.Date),
                InitialBalance = GetMontant(record.oldSolde.Solde, record.oldSolde.Signe),
                NewBalance = GetMontant(record.newSolde.Solde, record.newSolde.Signe),
                Transactions = trs,
            };

            return st;
        }

        internal static double GetMontant(string money, string signe)
        {
            var montant = double.Parse(money);
            if (signe == "1")
                montant *= -1;
            return montant / 1000;
        }

        private static Transactions GetTransaction(Mouvement mv)
        {
            if (mv.CodeOperation.StartsWith('1'))
                return null;
            if (mv.CodeOperation.StartsWith('3'))
                return null;

            var cb = new CompteBancaire
            {
                Bic = mv.Bic,
                Iban = mv.NumCompteContrepartie,
                //Titulaire = mv.NomCompteContrepartie,
                CurrencyCode = mv.DeviseCompteContrepartie,
            };

            var tr = new Transactions
            {
                Amount = GetMontant(mv.Montant, mv.Signe),
                CompteBancaire = cb,
                Message = mv.GetCommunication(),
                StructuredMessage = Txt(mv.CommunicationStructuree),
                TransactionDate = GetDate(mv.DateComptabilisation),
                ValueDate = GetDate(mv.Date),
            };

            return tr;
        }

        private static string Txt(string txt)
        {
            if (string.IsNullOrEmpty(txt))
                return string.Empty;
            return txt;
        }
    }
}
