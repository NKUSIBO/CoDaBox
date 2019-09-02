using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inocrea.CodaBox.ApiModel.Models;
using Inocrea.CodaBox.ApiServer.Models;
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

            //Hack: utiliser des fichier au lieu de string
            var jsonPdf = "{\"BE72735053527416\":\"g4xh19f16109faa164ef2ad191fe5604de2b3\",\"BE89731046687485\":\"g4xh1377ab270b8394a0ea449c2f82409b6cb\",\"BE72737046198416\":\"g4xh1a7fa26629b1a477098011b15bc71d673\",\"BE58735051972079\":\"f46qj1de1b36ffc314760a6e9bcb97e35a9b7\",\"BE67001857374487\":\"f46qj45493032a7e84f369bc9c4fb7ec5e1ce\",\"BE25735051972382\":\"f46qj363249babda846488ca7152da19961ad\",\"BE56001822027788\":\"g4xh1ec51b02b6d1141b7917fc368458fddad\",\"BE69736057182978\":\"g4xh1ec51b02b6d1141b7917fc368458fddad\",\"BE05735042087375\":\"c3c6h54dbd4d57ba54279aab64f9f6af86dac\",\"BE05734046071975\":\"c3c6he437301570184c4ba9f81000cde049de\",\"BE98035922514093\":\"c3c6h4b2e91d397454b9ba0a6b4bceb7ea08e\",\"BE35001677318037\":\"c3c6h0b41e38f9a3141c683e50c207575f14c\",\"BE55743065090044\":\"c3c6h5e1b0256cf424ebea46d6faaf325519b\",\"BE46001754141936\":\"g4xh16f3ba76fcc84416b9c5c1982c97425d2\",\"BE44743065090145\":\"c3c6h921cb25625b240e994e46184d7cccfc1\",\"BE02734042508540\":\"c3c6hc4d47a173e00425095dd991bbacecea5\",}";
            var jsonCod = "{\"BE72735053527416\":\"g4xh1bc4339a032764a8d81d80f4394f574a0\",\"BE89731046687485\":\"c3c6h1886e70f13ea4af19257703e9ca3daff\",\"BE72737046198416\":\"c3c6h2ede167189aa472794438b58d5bed5ae\",\"BE58735051972079\":\"f46qj6a468eccee814357a0919f8d2df7f2ee\",\"BE67001857374487\":\"f46qj3df46e434f814b9dbcb39bb0632cbda3\",\"BE25735051972382\":\"f46qjf6ad504b61bf4f079657b00a10de2efe\",\"BE56001822027788\":\"f8ut7efc42f764b2841629bbe8cc618ddbe5b\",\"BE69736057182978\":\"f8ut7efc42f764b2841629bbe8cc618ddbe5b\",\"BE05735042087375\":\"c3c6h998e38091fa34f1b9ea5b20772d3460b\",\"BE05734046071975\":\"c3c6hb179c7be91a04483a26e7a0b5609c5ed\",\"BE98035922514093\":\"c3c6h1a1bffb5e5df4911b101560fe0045d21\",\"BE35001677318037\":\"c3c6h00fd79a4cfed4c17b8a7182702598f5f\",\"BE55743065090044\":\"c3c6hdac6994d5bbb451ba23cd90532f31a5a\",\"BE46001754141936\":\"c3c6h6a803e4622cd49c1ba04a92528658b31\",\"BE44743065090145\":\"c3c6h6ad7201f1e7549b3a9cf58ca112157c2\",\"BE02734042508540\":\"c3c6hc477b953907249429bba03f61d8ad1f3\"}";

            var pdfPath = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonPdf);
            var codPath = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonCod);

            var exclu = string.Empty;

            List<Statements> feedStatements = new List<Statements>();
            foreach (var feed in feedEntries)
            {
                var md = feed.Metadata;
                var index = feed.FeedIndex;
                var name = md.NewBalanceDate.ToString("yyyy-MM-dd") + ' ' + md.Iban;

                var cod = string.Empty;
                var ok = true;
                //Hack: ceci exclu les coda autre que inocrea & co

                if (!pdfPath.ContainsKey(md.Iban) && !codPath.ContainsKey(md.Iban))
                {
                    exclu += name + '\n';
                    continue;
                }
                if (codPath.ContainsKey(md.Iban))
                {
                    //cod = client.GetCodaRedownFile(index, "cod");
                    cod = client.GetCodaFile(index, "cod");
                    var directory = codPath[md.Iban];
                    var codOk = ApiWD.UploadFile(cod, directory, name + ".cod").Result;
                    _ = ApiWD.UploadFile(cod, "g4xh16d20b8ee120a4156ba185622f0637fbb", name + ".cod");

                    if (!codOk)
                        exclu += name + '\n';
                }
                if (pdfPath.ContainsKey(md.Iban))
                {
                    //var pdf = client.GetCodaRedownFilePdf(index, "pdf");
                    var pdf = client.GetCodaFilePdf(index, "pdf");
                    var directory = pdfPath[md.Iban];
                    var pdfOk = ApiWD.UploadFile(pdf, directory, name + ".pdf").Result;
                    _ = ApiWD.UploadFile(pdf, "g4xh16d20b8ee120a4156ba185622f0637fbb", name + ".pdf");

                    if (!pdfOk)
                        exclu += name + '\n';
                }

                var statements = client.GetStatementsAsync(cod).Result;
                statements = SaveStatement(statements);

                client.PutFeed(id, index);

            }

            if (exclu != string.Empty)
                _ = ApiWD.UploadFile(exclu, "g4xh16d20b8ee120a4156ba185622f0637fbb", id.ToString() + ".txt");

            return feedStatements;
        }

        private IEnumerable<Statements> SaveStatement(IEnumerable<Statements> statements)
        {
            foreach (var st in statements)
            {
                st.CompteBancaire = BankAccount(st.CompteBancaire);
                foreach (var tr in st.Transactions)
                    tr.CompteBancaire = BankAccount(tr.CompteBancaire);

                List<TrasactionXls> trasactionXls = new List<TrasactionXls>();
                foreach (var tr in st.Transactions)
                    trasactionXls.Add(new TrasactionXls(tr));

                _ = Export.WriteTsvAsync(trasactionXls, st.Date.ToString("yyyy-MM-dd") + ' ' + st.CompteBancaire.Iban + ".xls");

                Db.Statements.Add(st);
                Db.SaveChanges();
            }
            return statements;
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