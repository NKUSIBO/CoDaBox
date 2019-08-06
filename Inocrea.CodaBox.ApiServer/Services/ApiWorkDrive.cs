using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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

        public async Task UploadFile(string txt, string fileName)
        {
            byte[] data = Encoding.UTF8.GetBytes(txt);
            var uri = new Uri(baseUrl + fileName);

            await PostFileAsync(uri, data, fileName);
        }

        public async Task UploadXls(MemoryStream stream, string fileName)
        {
            byte[] data = stream.ToArray();
            var uri = new Uri(baseUrl + fileName);

            await PostFileAsync(uri, data, fileName);
        }

    }
}
