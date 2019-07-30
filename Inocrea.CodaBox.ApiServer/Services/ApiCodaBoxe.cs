using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using CodaParser;

using System.Collections.Generic;
using Inocrea.CodaBox.ApiServer.Entities;

namespace Inocrea.CodaBox.ApiServer.Services
{
    public class ApiCodaBoxe
    {
        private HttpClient _httpClient;
        public ApiCodaBoxe()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("X-Software-Company", "641088c3-8fcb-47a3-8cef-de8197f5172c");
            var byteArray = Encoding.ASCII.GetBytes("GF-4e2cee89-e8df-4a1d-b285-f7c:XyJn6NQYrm");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        private async Task<string> GetAsync(Uri uri)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                Console.WriteLine("GET\t" + uri);
            }
            catch (Exception)
            {
                throw new ApiException(_httpClient, "GET", uri, response);
            }
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        private async Task<bool> PutAsync(Uri uri, HttpContent content)
        {
            HttpResponseMessage response = null;
            try
            {
                //todo put feed
                //response = await _httpClient.PutAsync(uri, content);
                //response.EnsureSuccessStatusCode();
                Console.WriteLine("PUT\t" + uri);
            }
            catch (Exception)
            {
                throw new ApiException(_httpClient, "PUT", uri, content, response);
            }
            return true;
        }

        public IEnumerable<FeedClient> GetPod()
        {
            var uri = new Uri("https://sandbox-api.codabox.com/v2/delivery/pod-client/");
            var data = GetAsync(uri).Result;
            var pod = JsonConvert.DeserializeObject<PodClient>(data);
            return pod.FeedClients;
        }

        public IEnumerable<FeedEntry> GetFeed(int id)
        {
            var uri = new Uri("https://sandbox-api.codabox.com/v2/delivery/feed/" + id + "/");
            var data = GetAsync(uri).Result;
            var feed = JsonConvert.DeserializeObject<Feed>(data);
            return feed.FeedEntries;
        }

        public IEnumerable<FeedEntry> GetRedownloadFeed(int id)
        {
            var uri = new Uri("https://sandbox-api.codabox.com/v2/delivery/redownload-feed/" + id + "/");
            var data = GetAsync(uri).Result;
            var feed = JsonConvert.DeserializeObject<Feed>(data);
            return feed.FeedEntries;
        }

        public IEnumerable<Statements> GetCoda(Guid index)
        {
            var uri = new Uri("https://sandbox-api.codabox.com/v2/delivery/download/" + index + "/cod/");
            var data = GetAsync(uri).Result;
            var line = data.Split('\n');
            var parser = new Parser();
            var statements = parser.Parse(line);

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

        public IEnumerable<Statements> GetCoda()
        {
            var index = new Guid("3b16ddcf-5ff4-45eb-a476-64202d2d3b58");
            return GetCoda(index);
        }

        public bool PutFeed(int id, Guid index)
        {
            var json = "{\"feed_offset\":\"" + index + "\"}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var uri = new Uri("https://sandbox-api.codabox.com/v2/delivery/feed/" + id + "/");
            var data = PutAsync(uri, content).Result;
            return data;
        }
    }
}
