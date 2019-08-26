using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Inocrea.CodaBox.ApiServer.Services
{
    public class ApiWorkDrive : ApiBase
    {
        private string baseUrl = "https://workdrive.zoho.com/api/v1/upload?";

        public ApiWorkDrive()
        {
            var zoho = new ApiZoho();
            SetRequestHeaders("Authorization", "Zoho-oauthtoken "+zoho.Token);
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
            throw new NotImplementedException();
        }

        public async Task<bool> UploadFile(Stream pdf, string id, string fileName)
        {
            var uri = new Uri(baseUrl + "parent_id=" + id + "&filename=" + fileName);

            return await PostFilePdfAsync(uri, pdf, fileName);
        }
    }
}
