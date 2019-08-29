using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Inocrea.CodaBox.Back.Services
{
    public class ApiWorkDrive : ApiBase
    {
        private string baseUrl = "https://workdrive.zoho.com/api/v1/upload?";

        public ApiWorkDrive()
        {
            var zoho = new ApiZoho();
            SetRequestHeaders("Authorization", "Zoho-oauthtoken " + zoho.Token);
        }

        public async Task<bool> UploadJson(string json)
        {
            byte[] data = Encoding.UTF8.GetBytes(json);
            var fileName = "coda" + ".json"; //declaration.json";
            var uri = new Uri(baseUrl + fileName);

            return await PostFileAsync(uri, data, fileName);
        }

        public async Task<bool> UploadFile(string txt, string id, string fileName)
        {
            byte[] data = Encoding.UTF8.GetBytes(txt);
            var uri = new Uri(baseUrl + "parent_id=" + id + "&filename=" + fileName);

            return await PostFileAsync(uri, data, fileName);
        }

        public async Task UploadXls(MemoryStream stream, string fileName)
        {
            var uri = new Uri(baseUrl + "parent_id=6j92v79240bb1f6d742aa9a98c72b6e85e937" + "&filename=" + fileName);
            await PostFileAsync(uri, stream, fileName);
        }

        public async Task<bool> UploadFile(Stream pdf, string id, string fileName)
        {
            var uri = new Uri(baseUrl + "parent_id=" + id + "&filename=" + fileName);
            return await PostFilePdfAsync(uri, pdf, fileName);
        }
    }
}
