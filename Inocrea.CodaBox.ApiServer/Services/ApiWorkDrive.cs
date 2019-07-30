using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Inocrea.CodaBox.ApiServer.Entities;

namespace Inocrea.CodaBox.ApiServer.Services
{
    public class ApiWorkDrive : ApiBase
    {
        private string baseUrl = "https://workdrive.zoho.com/api/v1/upload?parent_id=6j92v79240bb1f6d742aa9a98c72b6e85e937&filename=";
        private string token = "1000.b3c43fcf282607dc2e2339fb11bd1ec3.a964936a7b4aa2133e0d9c438c8de229";

        public ApiWorkDrive()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Zoho-oauthtoken "+token);
        }


        private async Task UploadJson(ICollection<Transactions> DataList)
        {
            if (DataList.Count <= 0) return;
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(DataList);

            byte[] data = Encoding.UTF8.GetBytes(json);
            var fileName = "coda" + ".json"; //declaration.json";
            var uri = new Uri(baseUrl + fileName);

            await PostAsync(uri, data, fileName);
        }


        public async Task UploadXml(MemoryStream stream)
        {
            byte[] data = stream.ToArray();
            var fileName = "coda" + ".xls"; //declaration.xls";
            var uri = new Uri(baseUrl + fileName);

            await PostAsync(uri, data, fileName);
        }
    }
}
