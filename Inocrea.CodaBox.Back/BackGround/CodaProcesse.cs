using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inocrea.CodaBox.ApiModel.Models;
using Inocrea.CodaBox.Back.Entities;
using Inocrea.CodaBox.Back.Models;
using Inocrea.CodaBox.Back.Services;
using Newtonsoft.Json;

namespace Inocrea.CodaBox.Back.BackGround
{
    public class CodaProcesse
    {
        private ApiCodaBoxe client;
        private InosysDBContext Db;

        public bool Start()
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
                //var feedEntries = client.GetRedownloadFeed(id);
                var feedEntries = client.GetFeed(id);
                var cod = GetCodasAsync(feedEntries, id);
                if (cod != null) allStatements.AddRange(cod);
            }
            return ok;
        }

        private IEnumerable<Statements> GetCodasAsync(IEnumerable<FeedEntry> feedEntries, int id)
        {
            var ApiWD = new ApiWorkDrive();
            var Path = "/Users/bilal/Downloads/Coda/";

            var jsonPdf = File.ReadAllText(Path + "PdfPath.json");
            var jsonCod = File.ReadAllText(Path + "CodPath.json");

            var pdfPath = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonPdf);
            var codPath = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonCod);

            List<Statements> feedStatements = new List<Statements>();
            foreach (var feed in feedEntries)
            {
                var md = feed.Metadata;
                var index = feed.FeedIndex;
                var name = md.NewBalanceDate.ToString("yyyy-MM-dd") + ' ' + md.Iban + ".cod";

                var cod=string.Empty;
                var ok = true;
                //Hack: ceci exclu les coda autre que inocrea & co

                if (!pdfPath.ContainsKey(md.Iban) && !codPath.ContainsKey(md.Iban)) continue;

                if (codPath.ContainsKey(md.Iban))
                {
                    //cod = client.GetCodaRedownFile(index, "cod");
                    cod = client.GetCodaFile(index, "cod");
                    var directory = codPath[md.Iban];
                    var codOk = ApiWD.UploadFile(cod, directory, name).Result;
                    if (!codOk) continue;
                }

                if (pdfPath.ContainsKey(md.Iban))
                {
                    //var pdf = client.GetCodaRedownFilePdf(index, "pdf");
                    var pdf = client.GetCodaFilePdf(index, "pdf");
                    var directory = pdfPath[md.Iban];
                    var pdfOk = ApiWD.UploadFile(pdf, directory, name).Result;
                    if (!pdfOk) continue;
                }

                //var statements = client.GetStatementsAsync(cod).Result;
                //statements = SaveStatement(statements);

                //client.PutFeed(id, index);

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