using Inocrea.CodaBox.ApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CodaParser;
using Inocrea.CodaBox.ApiModel.ViewModel;
using Inocrea.CodaBox.CodaApiClient.Helper;
using Newtonsoft.Json;
using CompteBancaire = Inocrea.CodaBox.ApiModel.CompteBancaire;

namespace Inocrea.CodaBox.CodaApiClient
{
    public partial class ApiClient
    {
        ApiServerCoda _api = new ApiServerCoda();
        string[] stringLine;
        private string rep = "";
        private string data = "";
        private static List<Statements> _statementToInsert = new List<Statements>();
        private static  List<string> IbanAccountToInsert = new List<string>();
        private static  List<string> IbanAccountInserted = new List<string>();

        public async Task<List<StatementAccountViewModel>> GetInvoice()
        {
          
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.CurrentCulture,
                ""));
            rep =  await  GetAsync<List<StatementAccountViewModel>>(requestUrl);
            stringLine = new string[] { rep };
            data = rep;
            //code to insert in db must be here
            WrittingToFile(stringLine);
            

            var stat = GetStatements();
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

        private void PostCompte(CompteBancaire compte)
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

            var compteToInsert = JsonConvert.SerializeObject(compte);
            var content = new StringContent(compteToInsert, System.Text.Encoding.UTF8, "application/json");
            try
            {
                if (IbanAccountToInsert.Count>0)
                {
                   
                    
                        
                            HttpResponseMessage result = client.PostAsync("api/CompteBancaires", content).Result;
                            if (result.IsSuccessStatusCode)
                            {
                                IbanAccountInserted.Add(compte.Iban);
                                Console.WriteLine("All is fine");

                            }
                    
                }
                else
                {
                    HttpResponseMessage result = client.PostAsync("api/CompteBancaires", content).Result;
                    if (result.IsSuccessStatusCode)
                    {
                       
                        IbanAccountInserted.Add(compte.Iban);
                        Console.WriteLine("All is fine");

                    }

                }
                
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }
        public async Task<CompteBancaire> GetCompte(int id)
        {
            CompteBancaire role = new CompteBancaire();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync(("api/CompteBancaires/" + id));
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                role = JsonConvert.DeserializeObject<CompteBancaire>(result);
            }

            return role;
        }
        private async Task<List<CompteBancaire>> GetAccount()
        {
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync(("api/CompteBancaires"));
            var comptes = new List<CompteBancaire>();

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                try
                {
                    comptes = JsonConvert.DeserializeObject<List<CompteBancaire>>(result);

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

            
            return comptes;
        }
        private List<StatementAccountViewModel> GetBusinessStatements( string data)

        {
            
            var parser = new Parser();
            var statementsFromDb = GetStatementsFromDbAsync(); ;
           
            foreach (var statement in statementsFromDb.Result)
            {
                StatementAccountViewModel sta = new StatementAccountViewModel();

                sta.Date = statement.Date;
                sta.InitialBalance = statement.InitialBalance;
                sta.NewBalance = statement.NewBalance;
                sta.Iban = statement.CompteBancaire.Iban;
                sta.IdentificationNumber = statement.CompteBancaire.IdentificationNumber;
                sta.Bic = statement.CompteBancaire.Bic;
                sta.InformationalMessage = statement.InformationalMessage;
                sta.CurrencyCode = statement.CompteBancaire.CurrencyCode;
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

        public async Task<List<Statements>> GetStatementsFromDbAsync()
        {
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/Statements");
            var statements = new List<Statements>();

            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;

                try
                {
                    statements = JsonConvert.DeserializeObject<List<Statements>>(result);
                    foreach (var statement in statements)
                    {
                        CompteBancaire cp = GetCompte(statement.CompteBancaireId).Result;
                        statement.CompteBancaire = cp;
                    }
                }
                catch ( Exception ex)
                {

                    throw ex;
                }
            }

            
            return statements;

        }
        public async Task<List<Statements>> GetStatements()

        {
            List<Statements> listStatement = new List<Statements>();

            CompteBancaire transCompte = new CompteBancaire();
            var parser = new Parser();
            var statements = parser.ParseFile(@"C:\Users\Public\TestFolder\WriteLines.cod");

            listStatement=await ParsingCod(statements);
            
            _statementToInsert = listStatement;
            return listStatement; 
            
        }
        public async Task<List<CompteBancaire>> GettingAccountToInsert(IEnumerable<CodaParser.Statements.Statement> statements)
        {
            List<CompteBancaire> listCompte = new List<CompteBancaire>();
            
            foreach (var statement in statements)
            {
                

                CompteBancaire cp = new CompteBancaire
                {
                    CurrencyCode = statement.Account.CurrencyCode,
                    Iban = statement.Account.Number,
                    IdentificationNumber = statement.Account.CompanyIdentificationNumber,
                    Bic = statement.Account.Bic
                };

                listCompte.Add(cp);
                foreach (var transaction in statement.Transactions)
                {


                               CompteBancaire transCompte = new CompteBancaire();

                                transCompte.Bic = transaction.Account.Bic;
                                transCompte.CurrencyCode = transaction.Account.CurrencyCode;
                                transCompte.Iban = transaction.Account.Number;
                                transCompte.IdentificationNumber = transaction.Account.Name;

                                var match = listCompte.Find(c => c.Iban == cp.Iban);
                                if (match == null) listCompte.Add(cp);


















                }



              
            }



            return listCompte;


        }

        public async Task<List<Statements>> ParsingCod(IEnumerable<CodaParser.Statements.Statement> statements)
        {
            List<Statements> listSta = new List<Statements>();
            List<Transactions> listTra = new List<Transactions>();

            foreach (var statement in statements)
            {
                CompteBancaire transCompte = new CompteBancaire();

                CompteBancaire cp = new CompteBancaire
                {
                    CurrencyCode = statement.Account.CurrencyCode,
                    Iban = statement.Account.Number,
                    IdentificationNumber = statement.Account.CompanyIdentificationNumber,
                    Bic = statement.Account.Bic
                };
                Statements sta = new Statements
                {
                    CompteBancaire = cp,
                    Date = statement.Date,
                    InitialBalance = statement.InitialBalance,
                    NewBalance = statement.NewBalance,
                    InformationalMessage = statement.Account.Name
                };

                foreach (var transaction in statement.Transactions)
                {
                    Transactions mytransaction = new Transactions
                    {
                        ValueDate = transaction.ValutaDate,
                        Amount = transaction.Amount,
                        StructuredMessage = transaction.StructuredMessage,
                        TransactionDate = transaction.TransactionDate
                    };
                    var listCp = await GetAccount();
                    if (listCp.Count > 0)
                    {
                        transCompte.Iban = transaction.Account.Number;
                        foreach (var tra in listCp)
                        {

                            if (transCompte.Iban != tra.Iban)
                            {
                                mytransaction.CompteBancaire = transCompte;
                                transCompte.Bic = transaction.Account.Bic;
                                transCompte.CurrencyCode = transaction.Account.CurrencyCode;
                                transCompte.Iban = transaction.Account.Number;
                                transCompte.IdentificationNumber = transaction.Account.Name;
                                var listTempCp = await GetAccount();
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


                        transCompte.Iban = transaction.Account.Number;
                        transCompte.Bic = transaction.Account.Bic;
                        transCompte.CurrencyCode = transaction.Account.CurrencyCode;


                        transCompte.Iban = transaction.Account.Number;
                        transCompte.IdentificationNumber = transaction.Account.Name;
                        if (!IbanAccountToInsert.Contains(transCompte.Iban))
                        {
                            IbanAccountToInsert.Add(transCompte.Iban);
                            
                        }
                        PostCompte(transCompte);
                        var listTempCp = await GetAccount();
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
            foreach (var item in _statementToInsert)
            {
               var t= SaveStatement(item);
            }
        }
    }
}
