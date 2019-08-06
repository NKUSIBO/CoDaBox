using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using Newtonsoft.Json;

using System.Collections.Generic;
using Inocrea.CodaBox.ApiServer.Entities;
using CodaParser;
using Inocrea.CodaBox.ApiServer.BackGround;
using System.Threading.Tasks;

namespace Inocrea.CodaBox.ApiServer.Services
{
    public class ApiCodaBoxe:ApiBase
    {
        //private string baseUrl = "https://sandbox-api.codabox.com/v2/delivery/";
        //private string xCompany = "641088c3-8fcb-47a3-8cef-de8197f5172c";
        //private string login = "GF-4e2cee89-e8df-4a1d-b285-f7c";
        //private string pwd = "XyJn6NQYrm";

        private string baseUrl = "https://api.codabox.com/v2/delivery/";
        private string xCompany = "e11e853b-97a9-4e63-9385-e4d5f5b84bed";
        private string login = "GF-2cdef81c-6e0e-4aba-850f-d50";
        private string pwd = "HZyUU7hS8q";

        public ApiCodaBoxe()
        {
            SetRequestHeaders("X-Software-Company", xCompany);
            var byteArray = Encoding.ASCII.GetBytes(login+':'+pwd);
            SetAuthorization (new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray)));
        }



        public IEnumerable<FeedClient> GetPod()
        {
            var uri = new Uri(baseUrl + "pod-client/");
            var data = GetAsync(uri).Result;
            PodClient podClient = null;
            try
            {
                podClient = JsonConvert.DeserializeObject<PodClient>(data);
            }
            catch(Exception e)
            {

            }
            return podClient.FeedClients;
        }

        public IEnumerable<FeedEntry> GetFeed(int id)
        {
            var uri = new Uri(baseUrl + "feed/" + id + "/");
            var data = GetAsync(uri).Result;
            var feed = JsonConvert.DeserializeObject<Feed>(data);
            return feed.FeedEntries;
        }

        public IEnumerable<FeedEntry> GetRedownloadFeed(int id)
        {
            var uri = new Uri(baseUrl+"redownload-feed/" + id + "/");
            var data = GetAsync(uri).Result;
            var feed = JsonConvert.DeserializeObject<Feed>(data);
            return feed.FeedEntries;
        }

        public string GetCodaFile(Guid index, string extension)
        {
            var uri = new Uri(baseUrl + "download/" + index + '/'+ extension + '/');
            var data = GetAsync(uri).Result;
            return data;
        }

        public async Task<IEnumerable<Statements>> GetStatementsAsync(string data)
        {
            data = data.Replace('\r','\n');
            var line = data.Split('\n');

            var parser = new Parser();
            IEnumerable<CodaParser.Statements.Statement> statements=null;
            try
            {
                statements = parser.Parse(line);
            }
            catch (Exception e)
            {

            }

            var sts = new List<Statements>();

            foreach (var st in statements)
            {

                var account = st.Account;
                var compteBancaire = new CompteBancaire
                {
                    Iban = account.Number.Replace(" ", ""),
                    Bic = account.Bic,
                    IdentificationNumber = account.CompanyIdentificationNumber,
                    CurrencyCode = account.CurrencyCode
                };

                var mySt = new Statements
                {
                    Date = st.Date,
                    InformationalMessage = st.InformationalMessage,
                    InitialBalance = (double)st.InitialBalance,
                    NewBalance = (double)st.NewBalance,
                    CompteBancaire = compteBancaire
                };

                foreach (var tr in st.Transactions)
                {
                    var trAccount = tr.Account;
                    var cb = new CompteBancaire
                    {
                        Iban = trAccount.Number.Replace(" ",""),
                        Bic = trAccount.Bic,
                        CurrencyCode = trAccount.CurrencyCode
                    };

                    var myTr = new Transactions
                    {
                        Amount = (double)tr.Amount,
                        //todo
                        //Message = tr.Message,
                        StructuredMessage = tr.StructuredMessage,
                        TransactionDate = tr.TransactionDate,
                        ValueDate = tr.ValutaDate,
                        CompteBancaire = cb
                    };
                    mySt.Transactions.Add(myTr);
                }
                sts.Add(mySt);
            }

            return sts;
        } 

        public bool PutFeed(int id, Guid index)
        {
            var json = "{\"feed_offset\":\"" + index + "\"}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var uri = new Uri(baseUrl+"feed/" + id + "/");
            var data = PutAsync(uri, content).Result;
            return data;
        }
    }
}
