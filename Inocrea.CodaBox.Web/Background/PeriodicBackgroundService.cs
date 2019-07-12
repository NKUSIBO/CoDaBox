using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Text;
using System.Collections.Generic;
using Inocrea.CodaBox.ApiModel;
using System.Net.Http;
using System.IO;
using ClosedXML.Excel;
using System.Data;
using Inocrea.CodaBox.Web.Helper;

namespace Inocrea.CodaBox.Web.Background
{
    public class PeriodicBackgroundService: BackgroundService
    {
        private CancellationTokenSource _cts;
        public static List<Transactions> DataList = new List<Transactions>();

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await UploadJson();
                    await ExecuteWork();

                    stoppingToken.ThrowIfCancellationRequested();

                    if (_cts == null || _cts.Token.IsCancellationRequested)
                    {
                        _cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                    }

                    await Task.Delay(TimeSpan.FromSeconds(5), _cts.Token);
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
            if (DataList.Count <= 0) return;
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(DataList);
            var fileName = "coda" + ".json"; //declaration.json";
            byte[] data = Encoding.UTF8.GetBytes(json);
            var formContent = new MultipartFormDataContent();
            formContent.Add(new StreamContent(new MemoryStream(data)), "content", fileName);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Zoho-oauthtoken 1000.a40422cd0de203a50acb94bd84ce9bfd.9ee7ca893456b41250838ea6d11a9581");
            var response = await client.PostAsync("https://workdrive.zoho.com/api/v1/upload?parent_id=6j92v79240bb1f6d742aa9a98c72b6e85e937&filename=cod.json", formContent);
        }


        private async Task Execute(MemoryStream stream)
        {
            try
            {

                byte[] data = stream.ToArray();

                var fileName = "coda" + ".xlsx"; //declaration.json";
                var formContent = new MultipartFormDataContent();
                formContent.Add(new StreamContent(new MemoryStream(data)), "content", fileName);
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", "Zoho-oauthtoken 1000.a40422cd0de203a50acb94bd84ce9bfd.9ee7ca893456b41250838ea6d11a9581");
                var response = await client.PostAsync("https://workdrive.zoho.com/api/v1/upload?parent_id=6j92v79240bb1f6d742aa9a98c72b6e85e937&filename=cod.xlsx", formContent);
            }
            catch (Exception e)
            {
                var ex = e;
            }
        }

        private async Task ExecuteWork()
        {
            DataTable dt = ExportToExcel.ExportGenericTransactions<List<InvoiceModel>>(DataList);
            for (int i = 0; i < dt.Columns.Count; i++)
            {

                if ((dt.Columns[i].ColumnName.ToString().Contains("DATE") ||
                     (dt.Columns[i].ColumnName.ToString().Contains("Date"))))
                {
                    var name = dt.Columns[i].ColumnName;
                    if (!(dt.Columns[i].ColumnName.ToString().Contains("FORMAT")))
                    {
                        dt.Columns.Remove(dt.Columns[i].ColumnName.ToString());
                    }

                }


            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    try
                    {
                        wb.SaveAs(stream);
                        await Execute(stream);
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
            }
        }

    }
}