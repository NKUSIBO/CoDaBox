using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Inocrea.CodaBox.ApiServer.Entities;

namespace Inocrea.CodaBox.ApiServer.Services
{
    public class ApiWorkDrive : ApiBase
    {
        private string baseUrl = "https://workdrive.zoho.com/api/v1/upload?parent_id=6j92v79240bb1f6d742aa9a98c72b6e85e937&filename=";

        public ApiWorkDrive()
        {
            var zoho = new ApiZoho();
            SetRequestHeaders("Authorization", "Zoho-oauthtoken "+zoho.Token);
        }

        public async Task UploadJson(string json)
        {
            byte[] data = Encoding.UTF8.GetBytes(json);
            var fileName = "coda" + ".json"; //declaration.json";
            var uri = new Uri(baseUrl + fileName);

            await PostFileAsync(uri, data, fileName);
        }

        public async Task UploadXml(MemoryStream stream)
        {
            byte[] data = stream.ToArray();
            var fileName = "coda" + ".xls"; //declaration.xls";
            var uri = new Uri(baseUrl + fileName);

            await PostFileAsync(uri, data, fileName);
        }

    }
}
