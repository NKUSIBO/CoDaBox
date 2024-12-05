using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inocrea.CodaBox.ApiModel.Models;
using Inocrea.CodaBox.ApiServer.Entities;
using Inocrea.CodaBox.ApiServer.Services;

namespace Inocrea.CodaBox.ApiServer.BackGround
{
    public class CodaProcesse
    {
        private ApiCodaBoxe client;
        private InosysDBContext Db;

        public async Task<bool> Start()
        {
            Db = new InosysDBContext();
            var ok = true;

            //todo foreach coda ID
            //pour recuperer les infos de tout les clients

            var codaId = Db.CodaIdentities.Find(1);

            List<Statements> allStatements = new List<Statements>();
            client = new ApiCodaBoxe(codaId);
            var feedClients = client.GetPod();
            foreach (var feed in feedClients)
            {
                var id = feed.Id;
                var feedEntries = client.GetFeed(id);
                var cod = GetCodasAsync(feedEntries, id);
                allStatements.AddRange(cod);
            }
            return ok;
        }

        private IEnumerable<Statements> GetCodasAsync(IEnumerable<FeedEntry> feedEntries, int id)
        {
            var ApiWD = new ApiWorkDrive();
            List<Statements> feedStatements = new List<Statements>();
            foreach (var feed in feedEntries)
            {
                var md = feed.Metadata;
                var index = feed.FeedIndex;
                var pdf = client.GetCodaFilePdf(index, "pdf");
                _ = ApiWD.UploadFilePdf(pdf, md.NewBalanceDate.ToString("yyyy-MM-dd") + ' ' + md.Iban + ".pdf");
                var cod = client.GetCodaFile(index, "cod");
                _ = ApiWD.UploadFile(cod, md.NewBalanceDate.ToString("yyyy-MM-dd") + ' ' + md.Iban + ".cod");
                var statements = client.GetStatementsAsync(cod).Result;

                statements = SaveStatement(statements);
                client.PutFeed(id, index);
            }
            return feedStatements;
        }

        private IEnumerable<Statements> SaveStatement(IEnumerable<Statements> statements)
        {
            List<Statements> Statements = new List<Statements>();
            foreach (var st in statements)
            {
                st.CompteBancaire = BankAccount(st.CompteBancaire);
                foreach (var tr in st.Transactions)
                    tr.CompteBancaire = BankAccount(tr.CompteBancaire);

                _ = Export.WriteTsvAsync(st.Transactions, st.Date.ToString("yyyy-MM-dd") + ' ' + st.CompteBancaire.Iban + ".xls");

                Db.Statements.Add(st);
                Db.SaveChanges();
            }
            return Statements;
        }

        private CompteBancaire BankAccount(CompteBancaire bankAccount)
        {
            var iban = bankAccount.Iban.Replace(" ", "");
            try
            {
                var ba = Db.CompteBancaire.First(b => b.Iban == iban);
                return ba;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            try
            {
                Db.CompteBancaire.Add(bankAccount);
                Db.SaveChanges();
                return Db.CompteBancaire.First(b => b.Iban == iban);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }
    }
}