using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.IO;
using Inocrea.CodaBox.ApiServer.Entities;
using System.ComponentModel;
using System.Linq;

using Inocrea.CodaBox.ApiServer.Services;
using Newtonsoft.Json;

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

                    await ExecuteWork();
                    await UploadJson();
                    stoppingToken.ThrowIfCancellationRequested();

                    if (_cts == null || _cts.Token.IsCancellationRequested)
                    {
                        _cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                    }

                    await Task.Delay(TimeSpan.FromDays(1), _cts.Token);
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
            var Db = new InosysDBContext();
            var statements = Db.Statements.ToList();
            var transactions = Db.Transactions.ToList();

            foreach (var st in statements)
            {
                var scb = Db.CompteBancaire.FirstOrDefault(c => c.Id == st.CompteBancaireId);
                st.CompteBancaire = scb;
                var tr = transactions.Where(t => t.StatementId == st.StatementId);
                foreach (var t in tr)
                {
                    var cb = Db.CompteBancaire.FirstOrDefault(c => c.Id == t.CompteBancaireId);
                    t.CompteBancaire = cb;
                    st.Transactions.Add(t);
                }
            }
            var json = JsonConvert.SerializeObject(statements);
            var apiWD = new ApiWorkDrive();

            await apiWD.UploadJson(json);
        }

        public string WriteTsv<T>(IEnumerable<T> data)
        {
            string output = "";

            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in props)
            {
                output += prop.DisplayName + '\t'; // header
            }
            output += '\n';
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
            await apiWD.UploadXls(stream, "coda.xls");

        }

    }
}
