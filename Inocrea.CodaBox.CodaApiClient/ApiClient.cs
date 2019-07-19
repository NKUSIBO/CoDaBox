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
using Newtonsoft.Json;

namespace Inocrea.CodaBox.CodaApiClient
{
    public partial class ApiClient
    {
        private static List<InvoiceModel> listInvoice = new List<InvoiceModel>();
        private static List<MyTransaction> listTransactions = new List<MyTransaction>();

        string[] stringLine;
        private readonly HttpClient _httpClient;
        private Uri BaseEndpoint { get; set; }
        public ApiClient(Uri baseEndpoint)
        {
            BaseEndpoint = baseEndpoint ?? throw new ArgumentNullException("baseEndpoint");
            _httpClient = new HttpClient();
        }
        private async Task<List<Statements>> GetAsync<T>(Uri requestUrl)
        {
             AddHeaders();
             HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);

            }
            catch (Exception ex)
            {
                
                throw ex;
            }            
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            stringLine = new string[] { data };
            var parser = new Parser();
            WrittingToFile(stringLine);
            List<Statements> res = new List<Statements>();

            var statements = parser.ParseFile(@"C:\Users\Public\TestFolder\WriteLines.cod");

            foreach (var statement in statements)
            {
                List<MyTransaction> mytransList = new List<MyTransaction>();

                ApiModel.Statements st = new ApiModel.Statements();
                CompteBancaire cb = new CompteBancaire()
                {
                    Iban = statement.Account.Number,
                    IdentificationNumber = statement.Account.CompanyIdentificationNumber,
                    Bic = statement.Account.Bic,
                    CurrencyCode = statement.Account.CurrencyCode


                };
                st.CompteBancaire = cb;
                st.InitialBalance = statement.InitialBalance;
                st.Date = statement.Date;
                st.NewBalance = statement.NewBalance;
                st.InformationalMessage = statement.InformationalMessage;

               
                // modele à effacer

                InvoiceModel invoice = new InvoiceModel();
                invoice.AccountingDate = statement.Date.ToString("dd-MM-yyyy");
                invoice.InitialBalance = statement.InitialBalance;
                invoice.NewBalance = statement.NewBalance;
                invoice.Number = statement.Account.Number;
                invoice.NumeroIdentification = statement.Account.CompanyIdentificationNumber;
                invoice.Bic = statement.Account.Bic;
                invoice.Name = statement.Account.Name;

                invoice.CurrencyCode = statement.Account.CurrencyCode;
                foreach (var transaction in statement.Transactions)
                {
                    MyTransaction mytransaction = new MyTransaction();

                    CompteBancaire transCompte = new CompteBancaire();
                    transCompte.Bic = transaction.Account.Bic;
                    transCompte.CurrencyCode = transaction.Account.CurrencyCode;
                    transCompte.Iban = transaction.Account.Number;
                    transCompte.IdentificationNumber = transaction.Account.Name;
                    mytransaction.ValueDate = transaction.ValutaDate;
                    mytransaction.Amount = transaction.Amount;
                    mytransaction.StructuredMessage = transaction.StructuredMessage;
                    mytransaction.TransactionDate = transaction.TransactionDate;
                   
                    mytransaction.CompteBancaire = transCompte;
                    mytransList.Add(mytransaction);
                    ///modele à effacer
                    Transactions trans = new Transactions();

                    listTransactions.Add(mytransaction);
                    listInvoice.Add(invoice);
                    Console.WriteLine(transaction.Account.Name + ": " + transaction.Amount);
                }
                //invoice.Transactions = listTransactions;
                st.Transactions = listTransactions;
                res.Add(st);
                Console.WriteLine(statement.NewBalance);

            }
            //foreach (var statement in statements)
            //{
            //    InvoiceModel invoice = new InvoiceModel();
            //    invoice.AccountingDate = statement.Date.ToString("dd-MM-yyyy");
            //    invoice.InitialBalance = statement.InitialBalance;
            //    invoice.NewBalance = statement.NewBalance;
            //    invoice.Number = statement.Account.Number;
            //    invoice.NumeroIdentification = statement.Account.CompanyIdentificationNumber;
            //    invoice.Bic = statement.Account.Bic;
            //    invoice.Name = statement.Account.Name;
                
            //    invoice.CurrencyCode = statement.Account.CurrencyCode;
            //    foreach (var transaction in statement.Transactions)
            //    {
            //        Transactions trans= new Transactions();
            //        trans.AccountingDate = statement.Date.ToString("dd-MM-yyyy");
            //        trans.InitialBalance = statement.InitialBalance;
            //        trans.NewBalance = statement.NewBalance;
            //        trans.Number = statement.Account.Number;
            //        trans.NumeroIdentification = statement.Account.CompanyIdentificationNumber;
            //        trans.Bic = statement.Account.Bic;
            //        trans.Name = statement.Account.Name;
            //        trans.CurrencyCode = statement.Account.CurrencyCode;
            //        trans.Message = Regex.Replace(transaction.Message, @"    ", "");
            //        trans.StructuredMessage = transaction.StructuredMessage;
            //        trans.TransactionDate = transaction.TransactionDate.ToString("dd-MM-yyyy");

            //        trans.ValueDate = transaction.ValutaDate.ToString("dd-MM-yyyy");
            //        trans.Amount = transaction.Amount.ToString(CultureInfo.InvariantCulture);

            //        listTransactions.Add(trans);
            //        listInvoice.Add(invoice);
            //        Console.WriteLine(transaction.Account.Name + ": " + transaction.Amount);
            //    }
            //    invoice.Transactions = listTransactions;
                
            //    Console.WriteLine(statement.NewBalance);

            //}
            response.EnsureSuccessStatusCode();
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);

            return res;
            
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

            _httpClient.DefaultRequestHeaders.Remove("X-Software-Company");
            _httpClient.DefaultRequestHeaders.Add("X-Software-Company", "641088c3-8fcb-47a3-8cef-de8197f5172c");
            var byteArray = Encoding.ASCII.GetBytes("GF-4e2cee89-e8df-4a1d-b285-f7c:XyJn6NQYrm");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

   
    }
}
