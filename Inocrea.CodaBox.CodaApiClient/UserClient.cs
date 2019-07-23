using Inocrea.CodaBox.ApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CodaParser;
using Inocrea.CodaBox.ApiModel.ViewModel;
using Inocrea.CodaBox.CodaApiClient.Helper;
using Newtonsoft.Json;

namespace Inocrea.CodaBox.CodaApiClient
{
    public partial class ApiClient
    {
        ApiServerCoda _api = new ApiServerCoda();
        string[] stringLine;
        private string rep = "";
        private string data = "";
        private static List<Statements> listSta = new List<Statements>();
        private static List<Transactions> listTra= new List<Transactions>();

        public async Task<List<StatementAccountViewModel>> GetInvoice()
        {
          
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.CurrentCulture,
                ""));
            rep =  await  GetAsync<List<StatementAccountViewModel>>(requestUrl);
            stringLine = new string[] { rep };
            data = rep;
            WrittingToFile(stringLine);


            var stat = GetBusinessStatements(rep);
            return GetBusinessStatements
                (rep);
        }
        public void WrittingToFile(string[] lines)
        {

            // These examples assume a "C:\Users\Public\TestFolder" folder on your machine.
            // You can modify the path if necessary.


            // Example #1: Write an array of strings to a file.
            // Create a string array that consists of three lines.

            // WriteAllLines creates a file, writes a collection of strings to the file,
            // and then closes the file.  You do NOT need to call Flush() or Close().
            System.IO.File.Delete(@"C:\Users\Public\TestFolder\WriteLines.cod");
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

                foreach (string line in lines)
                {
                    // If the line doesn't contain the word 'Second', write the line to the file.
                    if (line.Contains("0000026061900005"))
                    {
                        Console.WriteLine(line);
                    }

                    // If the line doesn't contain the word 'Second', write the line to the file.
                    if (!line.Contains("0000026061900005"))
                    {
                        file.WriteLine(line);
                    }
                }
            }
        }

        private async Task  PostCompte(CompteBancaire compte)
        {
            HttpClient client = _api.Initial();
            //List<CompteBancaire> listIban = new List<CompteBancaire>();

            //var staInsertResult = await GetStatements();
            //foreach (var statement in staInsertResult)
            //{
            //    listIban.Add(statement.CompteBancaire);
            //    listIban.Add(statement.Transactions.FirstOrDefault()?.CompteBancaire);
            //}

            //listIban.Distinct().ToList();
           
                var statementToInsert = JsonConvert.SerializeObject(compte);
                var content = new StringContent(statementToInsert, System.Text.Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage result = client.PostAsync("api/CompteBancaires", content).Result;
                    if (result.IsSuccessStatusCode)
                    {

                        Console.WriteLine("All is fine");

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            

        }
        private async Task<List<CompteBancaire>> GetAccount()
        {
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync(("api/CompteBancaires"));
            var comptes = new List<CompteBancaire>();

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                comptes = JsonConvert.DeserializeObject<List<CompteBancaire>>(result);
            }

            List<CompteBancaire> listIban = new List<CompteBancaire>();
            
            return comptes;
        }
        private List<StatementAccountViewModel> GetBusinessStatements( string data)

        {
            
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
        public async Task<List<Statements>> GetStatements()

        {
            CompteBancaire transCompte = new CompteBancaire();
            var parser = new Parser();
            var statements = parser.ParseFile(@"C:\Users\Public\TestFolder\WriteLines.cod");

            foreach (var statement in statements)
            {
                CompteBancaire cp=new CompteBancaire();
                cp.CurrencyCode = statement.Account.CurrencyCode;
                cp.Iban = statement.Account.Number;
                cp.IdentificationNumber = statement.Account.CompanyIdentificationNumber;
                cp.Bic = statement.Account.Bic;
                Statements sta = new Statements();
                sta.CompteBancaire = cp;
                sta.Date = statement.Date;
                sta.InitialBalance = statement.InitialBalance;
                sta.NewBalance = statement.NewBalance;
                sta.InformationalMessage = statement.Account.Name;
              
                foreach (var transaction in statement.Transactions)
                {
                    Transactions mytransaction = new Transactions();
                    mytransaction.ValueDate = transaction.ValutaDate;
                    mytransaction.Amount = transaction.Amount;
                    mytransaction.StructuredMessage = transaction.StructuredMessage;
                    mytransaction.TransactionDate = transaction.TransactionDate;
                    List<CompteBancaire> listCp=new List<CompteBancaire>();
                    listCp = await GetAccount();
                    if (listCp.Count > 0)
                    {
                        foreach (var tra in listCp)
                        {

                            if (transCompte.Iban != tra.Iban)
                            {
                                CompteBancaire transCompte2 = new CompteBancaire();
                                mytransaction.CompteBancaire = transCompte;
                                transCompte.Bic = transaction.Account.Bic;
                                transCompte.CurrencyCode = transaction.Account.CurrencyCode;
                                transCompte.Iban = transaction.Account.Number;
                                transCompte.IdentificationNumber = transaction.Account.Name;
                                await PostCompte(transCompte2);
                                List<CompteBancaire> listTempCp = new List<CompteBancaire>();
                                listTempCp = await GetAccount();
                                var differences = listCp.Except(listTempCp);
                                CompteBancaire first = null;
                                foreach (var difference in differences)
                                {
                                    first = difference;
                                    break;
                                }

                                if (first != null) transCompte.Id = first.Id;
                            }
                            else mytransaction.CompteBancaireId = tra.Id;
                        }
                        
                    }
                    else
                    {
                      

                        transCompte.Bic = transaction.Account.Bic;
                        transCompte.CurrencyCode = transaction.Account.CurrencyCode;
                        transCompte.Iban = transaction.Account.Number;
                        transCompte.IdentificationNumber = transaction.Account.Name;
                        await PostCompte(transCompte);
                        List<CompteBancaire> listTempCp = new List<CompteBancaire>();
                        listTempCp = await GetAccount();
                        CompteBancaire first = null;
                        foreach (var bancaire in listTempCp)
                        {
                            first = bancaire;
                            break;
                        }

                        if (first != null) 
                         mytransaction.CompteBancaireId = first.Id;
                    }
                   
                   
                    listTra.Add(mytransaction);
                }

               
              
                sta.Transactions = listTra;
                listSta.Add(sta);

            }

            return listSta;
        }
     
        public async Task<Message<Statements>> SaveStatement(Statements model)
        {
           
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.CurrentCulture,
                ""));
            return await PostAsync<Statements>(requestUrl, model);
        }

        private void Save()
        {
            foreach (var item in listSta)
            {
               var t= SaveStatement(item);
            }
        }
    }
}
