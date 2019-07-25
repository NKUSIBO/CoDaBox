using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodaParser;
using Inocrea.CodaBox.ApiModel;
using Inocrea.CodaBox.ApiModel.ViewModel;
using Newtonsoft.Json;

namespace Inocrea.CodaBox.CodaApiClient
{
    public partial class ApiClient
    {
       
        private static List<InvoiceModel> listInvoice = new List<InvoiceModel>();
        private static List<Transactions> listTransactions = new List<Transactions>();
        private static List<StatementAccountViewModel> listStateAccountViewModels = new List<StatementAccountViewModel>();
        List<Statements> listStatements;
        string[] stringLineErase;
        private readonly HttpClient _httpClient;
        private Uri BaseEndpoint { get; set; }
        public ApiClient(Uri baseEndpoint)
        {
            BaseEndpoint = baseEndpoint ?? throw new ArgumentNullException("baseEndpoint");
            _httpClient = new HttpClient();
        }
        private List<Statements> GetStatements<T>(Uri requestUrl, string data)

        {
            //await CallingHelper(requestUrl);
            stringLine = new string[] { data };
            var parser = new Parser();
            var statements = parser.ParseFile(@"C:\Users\Public\TestFolder\WriteLines.cod");

          
            foreach (var statement in statements)
            {
                Statements sta = new Statements();

                sta.Date = statement.Date;
                sta.InitialBalance = statement.InitialBalance;
                sta.NewBalance = statement.NewBalance;
                sta.CompteBancaire.Iban = statement.Account.Number;
                sta.CompteBancaire.IdentificationNumber = statement.Account.CompanyIdentificationNumber;
                sta.CompteBancaire.Bic = statement.Account.Bic;
                sta.InformationalMessage = statement.InformationalMessage;
                sta.CompteBancaire.CurrencyCode = statement.Account.CurrencyCode;
                foreach (var tra in statement.Transactions)
                {

                }
                //invoice.CurrencyCode = statement.Account.CurrencyCode;
                //foreach (var transaction in statement.Transactions)
                //{
                //    Transactions trans = new Transactions();
                //    CompteBancaire transCompte = new CompteBancaire();
                //    transCompte.Bic = transaction.Account.Bic;
                //    transCompte.CurrencyCode = transaction.Account.CurrencyCode;
                //    transCompte.Iban = transaction.Account.Number;
                //    transCompte.IdentificationNumber = transaction.Account.Number;
                //    trans.AccountingDate = statement.Date.ToString("dd-MM-yyyy");
                //    trans.InitialBalance = statement.InitialBalance;
                //    trans.NewBalance = statement.NewBalance;
                //    trans.Number = statement.Account.Number;
                //    trans.NumberCustomer = transaction.Account.Number;
                //    trans.NumeroIdentification = statement.Account.CompanyIdentificationNumber;
                //    trans.Bic = statement.Account.Bic;
                //    trans.BicCustomer = transaction.Account.Bic;
                //    trans.Name = statement.Account.Name;
                //    trans.CurrencyCode = statement.Account.CurrencyCode;
                //    trans.Message = Regex.Replace(transaction.Message, @"    ", "");
                //    trans.StructuredMessage = transaction.StructuredMessage;
                //    trans.TransactionDate = transaction.TransactionDate.ToString("dd-MM-yyyy");

                //    trans.ValueDate = transaction.ValutaDate;
                //    trans.Amount = transaction.Amount.ToString(CultureInfo.InvariantCulture);

                //    listTransactions.Add(trans);
                //    listInvoice.Add(invoice);
                //    Console.WriteLine(transaction.Account.Name + ": " + transaction.Amount);
                //}
                //invoice.Transactions = listTransactions;
                listStatements.Add(sta);

            }

            return listStatements;
        }
        private  List<StatementAccountViewModel> GetBusinessStatements<T>(Uri requestUrl, string data)

        {
            //await CallingHelper(requestUrl);
            stringLine = new string[] { data };
            var parser = new Parser();
            var statements = parser.ParseFile(@"C:\Users\Public\TestFolder\WriteLines.cod");

            foreach (var statement in statements)
            {
                StatementAccountViewModel sta = new StatementAccountViewModel();

                sta.Date = statement.Date;
                sta.InitialBalance = statement.InitialBalance;
                sta.NewBalance = statement.NewBalance;
                sta.Iban = statement.Account.Number;
                sta.IdentificationNumber = statement.Account.CompanyIdentificationNumber;
                sta.Bic = statement.Account.Bic;
                sta.InformationalMessage = statement.Account.Name;
                sta.CurrencyCode = statement.Account.CurrencyCode;
                foreach (var tra in statement.Transactions)
                {
                    
                }
                //invoice.CurrencyCode = statement.Account.CurrencyCode;
                //foreach (var transaction in statement.Transactions)
                //{
                //    Transactions trans = new Transactions();
                //    CompteBancaire transCompte = new CompteBancaire();
                //    transCompte.Bic = transaction.Account.Bic;
                //    transCompte.CurrencyCode = transaction.Account.CurrencyCode;
                //    transCompte.Iban = transaction.Account.Number;
                //    transCompte.IdentificationNumber = transaction.Account.Number;
                //    trans.AccountingDate = statement.Date.ToString("dd-MM-yyyy");
                //    trans.InitialBalance = statement.InitialBalance;
                //    trans.NewBalance = statement.NewBalance;
                //    trans.Number = statement.Account.Number;
                //    trans.NumberCustomer = transaction.Account.Number;
                //    trans.NumeroIdentification = statement.Account.CompanyIdentificationNumber;
                //    trans.Bic = statement.Account.Bic;
                //    trans.BicCustomer = transaction.Account.Bic;
                //    trans.Name = statement.Account.Name;
                //    trans.CurrencyCode = statement.Account.CurrencyCode;
                //    trans.Message = Regex.Replace(transaction.Message, @"    ", "");
                //    trans.StructuredMessage = transaction.StructuredMessage;
                //    trans.TransactionDate = transaction.TransactionDate.ToString("dd-MM-yyyy");

                //    trans.ValueDate = transaction.ValutaDate;
                //    trans.Amount = transaction.Amount.ToString(CultureInfo.InvariantCulture);

                //    listTransactions.Add(trans);
                //    listInvoice.Add(invoice);
                //    Console.WriteLine(transaction.Account.Name + ": " + transaction.Amount);
                //}
                //invoice.Transactions = listTransactions;
                listStateAccountViewModels.Add(sta);

            }

            return listStateAccountViewModels;
        }

        
        private async Task<string> GetAsync<T> (Uri requestUrl)
        {
             AddHeaders();
             HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var data =  response.Content.ReadAsStringAsync().Result;
               
                 GetBusinessStatements<List<StatementAccountViewModel>>(requestUrl, data);

                response.EnsureSuccessStatusCode();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);
               
                return data;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }            
           
           
            
        }
        public void CleanToFile(string[] lines)
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
            stringLineErase = new string[] { "" };
            ;
            // Example #3: Write only some strings in an array to a file.
            // The using statement automatically flushes AND CLOSES the stream and calls 
            // IDisposable.Dispose on the stream object.
            // NOTE: do not use FileStream for text files because it writes bytes, but StreamWriter
            // encodes the output as text.
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:\Users\Public\TestFolder\WriteLines2.txt"))
            {
                foreach (var item in stringLineErase)
                {
                    file.Write(string.Empty);
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
        private HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
        /// <summary>
        /// Common method for making POST calls
        /// </summary>
        private async Task<Message<T>> PostAsync<T>(Uri requestUrl, T content)
        {
            AddHeaders();
            var response = await _httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T>(content));
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Message<T>>(data);
        }
        private async Task<Message<T1>> PostAsync<T1, T2>(Uri requestUrl, T2 content)
        {
            AddHeaders();
            var response = await _httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T2>(content));
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Message<T1>>(data);
        }

        private void AddHeaders()
        {

            _httpClient.DefaultRequestHeaders.Remove("X-Software-Company");
            _httpClient.DefaultRequestHeaders.Add("X-Software-Company", "641088c3-8fcb-47a3-8cef-de8197f5172c");
            var byteArray = Encoding.ASCII.GetBytes("GF-4e2cee89-e8df-4a1d-b285-f7c:XyJn6NQYrm");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

   
    }
}
