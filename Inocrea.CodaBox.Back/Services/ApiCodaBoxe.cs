using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using Newtonsoft.Json;

using System.Collections.Generic;
using Inocrea.CodaBox.Back.Models;
using Inocrea.CodaBox.ApiModel.Models;
using DeCoda;
using System.Threading.Tasks;
using System.IO;

namespace Inocrea.CodaBox.Back.Services
{
    public class ApiCodaBoxe:ApiBase
    {
        //private string baseUrl = "https://sandbox-api.codabox.com/v2/delivery/";
        //private string xCompany = "641088c3-8fcb-47a3-8cef-de8197f5172c";
        //private string login = "GF-4e2cee89-e8df-4a1d-b285-f7c";
        //private string pwd = "XyJn6NQYrm";

        private string baseUrl = "https://api.codabox.com/v2/delivery/";
        private CodaIdentity codaID;

        public ApiCodaBoxe(CodaIdentity codaID)
        {
            this.codaID = codaID;
            SetRequestHeaders("X-Software-Company", codaID.XCompany);
            var byteArray = Encoding.ASCII.GetBytes(codaID.Login+':'+codaID.Pwd);
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

        public string GetCodaRedownFile(Guid index, string extension)
        {
            var uri = new Uri(baseUrl + "redownload/" + index + '/' + extension + '/');
            var data = GetAsync(uri).Result;
            return data;
        }

        public Stream GetCodaRedownFilePdf(Guid index, string extension)
        {
            var uri = new Uri(baseUrl + "redownload/" + index + '/' + extension + '/');
            var data = GetStreamAsync(uri).Result;
            return data;
        }

        public string GetCodaFile(Guid index, string extension)
        {
            var uri = new Uri(baseUrl + "download/" + index + '/'+ extension + '/');
            var data = GetAsync(uri).Result;
            return data;
        }

        public Stream GetCodaFilePdf(Guid index, string extension)
        {
            var uri = new Uri(baseUrl + "download/" + index + '/' + extension + '/');
            var data = GetStreamAsync(uri).Result;
            return data;
        }

        public async Task<IEnumerable<Statements>> GetStatementsAsync(string data)
        {
            data = data.Replace('\r','\n');
            var line = data.Split('\n');

            var parser = new DeCoda.DeCoda();
            var sts = new List<Statements>();
            sts.Add(parser.getStatement(line));

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