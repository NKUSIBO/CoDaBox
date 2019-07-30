using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.IO;
using Inocrea.CodaBox.ApiServer.Entities;
using System.Net.Http;
using System.Text;
using System.ComponentModel;
using System.Linq;

using Inocrea.CodaBox.ApiServer.Services;

namespace Inocrea.CodaBox.ApiServer.BackGround
{
    public class PeriodicBackgroundService : BackgroundService
    {
        private CancellationTokenSource _cts;
        public static List<Transactions> DataList = new List<Transactions>();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var coda = new CodaProcesse();

                    //coda.Start();
                    //await UploadJson();
                    await ExecuteWork();




                    stoppingToken.ThrowIfCancellationRequested();

                    if (_cts == null || _cts.Token.IsCancellationRequested)
                    {
                        _cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                    }

                    await Task.Delay(TimeSpan.FromHours(1), _cts.Token);
                    stoppingToken.ThrowIfCancellationRequested();

                }
                catch (OperationCanceledException)
                {
                    // Log errors
                }
            }
        }

        private async Task UploadJson()
        {
            //if (DataList.Count <= 0) return;
            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(DataList);
            //var fileName = "coda" + ".json"; //declaration.json";
            //byte[] data = Encoding.UTF8.GetBytes(json);

            //var formContent = new MultipartFormDataContent();
            //formContent.Add(new StreamContent(new MemoryStream(data)), "content", fileName);
            //var client = new HttpClient();
            //client.DefaultRequestHeaders.Add("Authorization", "Zoho-oauthtoken 1000.973023507b1b3c7ef6125e0ffe1b4f48.2145f45f88073756b8a037f3be0bf2c8");
            //var response = await client.PostAsync("https://workdrive.zoho.com/api/v1/upload?parent_id=6j92v79240bb1f6d742aa9a98c72b6e85e937&filename=cod.json", formContent);
        }

        public string WriteTsv<T>(IEnumerable<T> data)
        {
            string output = "";

            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in props)
            {
                output += prop.DisplayName+ '\t'; // header
            }
            output+='\n';
            foreach (T item in data)
            {
                foreach (PropertyDescriptor prop in props)
                {
                    output += prop.Converter.ConvertToString(prop.GetValue(item)) + '\t';
                }
                output += '\n';
            }
            return output;
        }

        private async Task ExecuteWork()
        {
            var Db = new InosysDBContext();
            var transactions = Db.Transactions.ToList();
            var output = WriteTsv(transactions);

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(output);
            writer.Flush();
            stream.Position = 0;

            var apiWD = new ApiWorkDrive();
            
            await apiWD.UploadXml(stream);

        }

    }
}