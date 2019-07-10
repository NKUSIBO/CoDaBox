using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CodaParser;
using Inocrea.CodaBox.ApiModel;
using Newtonsoft.Json;

namespace Inocrea.CodaBox.CodaApiClient
{
    public partial class ApiClient
    {
        private static List<InvoiceModel> listInvoice = new List<InvoiceModel>();

        string[] stringLine;
        private readonly HttpClient _httpClient;
        private Uri BaseEndpoint { get; set; }
        public ApiClient(Uri baseEndpoint)
        {
            BaseEndpoint = baseEndpoint ?? throw new ArgumentNullException("baseEndpoint");
            _httpClient = new HttpClient();
        }
        private async Task<List<InvoiceModel>> GetAsync<T>(Uri requestUrl)
        {
            AddHeaders();
            var response = await _httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
            
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            stringLine = new string[] { data };
            var parser = new Parser();
            WrittingToFile(stringLine);
            var statements = parser.ParseFile(@"C:\Users\Public\TestFolder\WriteLines.cod");

            foreach (var statement in statements)
            {
                InvoiceModel invoice = new InvoiceModel();
                invoice.AccountingDate = statement.Date.ToString("yyyy -MM-dd");
                Console.WriteLine(statement.Date.ToString("yyyy -MM-dd"));

                foreach (var transaction in statement.Transactions)
                {
                    invoice.TransactionMessage = transaction.Account.Name;
                    invoice.Amount = transaction.Amount;
                    listInvoice.Add(invoice);
                    Console.WriteLine(transaction.Account.Name + ": " + transaction.Amount);
                }
                invoice.Balance = statement.NewBalance;
                Console.WriteLine(statement.NewBalance);

            }
            response.EnsureSuccessStatusCode();
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);

            return listInvoice;
            
        }
        public void WrittingToFile(string[] lines)
        {

            // These examples assume a "C:\Users\Public\TestFolder" folder on your machine.
            // You can modify the path if necessary.


            // Example #1: Write an array of strings to a file.
            // Create a string array that consists of three lines.

            // WriteAllLines creates a file, writes a collection of strings to the file,
            // and then closes the file.  You do NOT need to call Flush() or Close().
            System.IO.File.WriteAllLines(@"C:\Users\Public\TestFolder\WriteLines.cod", lines);


            // Example #2: Write one string to a text file.
            string text = "A class is the most powerful data type in C#. Like a structure, " +
                          "a class defines the data and behavior of the data type. ";
            // WriteAllText creates a file, writes the specified string to the file,
            // and then closes the file.    You do NOT need to call Flush() or Close().
            System.IO.File.WriteAllText(@"C:\Users\Public\TestFolder\WriteText.txt", text);

            // Example #3: Write only some strings in an array to a file.
            // The using statement automatically flushes AND CLOSES the stream and calls 
            // IDisposable.Dispose on the stream object.
            // NOTE: do not use FileStream for text files because it writes bytes, but StreamWriter
            // encodes the output as text.
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:\Users\Public\TestFolder\WriteLines2.txt"))
            {
                foreach (string line in lines)
                {
                    // If the line doesn't contain the word 'Second', write the line to the file.
                    if (!line.Contains("Second"))
                    {
                        file.WriteLine(line);
                    }
                }
            }
        }
        private static JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }
        private Uri CreateRequestUri(string relativePath, string queryString = "")
        {
            var endpoint = new Uri(BaseEndpoint, relativePath);
            var uriBuilder = new UriBuilder(endpoint) {Query = queryString};
            return uriBuilder.Uri;
        }
        private void AddHeaders()
        {
            
            
            _httpClient.DefaultRequestHeaders.Add("X-Software-Company", "641088c3-8fcb-47a3-8cef-de8197f5172c");
            var byteArray = Encoding.ASCII.GetBytes("GF-4e2cee89-e8df-4a1d-b285-f7c:XyJn6NQYrm");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

   
    }
}
