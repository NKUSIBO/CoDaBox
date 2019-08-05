using System;
using System.Collections.Generic;
using System.Linq;
using Inocrea.CodaBox.ApiServer.Entities;
using Inocrea.CodaBox.ApiServer.Services;
using CompteBancaire = Inocrea.CodaBox.ApiServer.Entities.CompteBancaire;
using Statements = Inocrea.CodaBox.ApiServer.Entities.Statements;

namespace Inocrea.CodaBox.ApiServer.BackGround
{
    public class CodaProcesse
    {
        private ApiCodaBoxe client;
        private InosysDBContext Db;
        private string xxx;
        public bool Start()
        {
            Db = new InosysDBContext();
            var ok = true;
            List<Statements> allStatements = new List<Statements>();
            client = new ApiCodaBoxe();
            var feedClients = client.GetPod();
            foreach (var feed in feedClients)
            {
                var id = feed.Id;
                var feedEntries = client.GetFeed(id);
                var cod = GetCodas(feedEntries, id);
                allStatements.AddRange(cod);
            }
            return ok;
        }

        private IEnumerable<Statements> GetCodas(IEnumerable<FeedEntry> feedEntries, int id)
        {
            List<Statements> feedStatements = new List<Statements>();
            foreach (var feed in feedEntries)
            {
                var index = feed.FeedIndex;
                var statements = client.GetCoda(index);
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
                {
                    tr.CompteBancaire = BankAccount(tr.CompteBancaire);
                }
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