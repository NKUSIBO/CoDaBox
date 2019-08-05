using Inocrea.CodaBox.ApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using System.Threading.Tasks;

using Inocrea.CodaBox.ApiModel.ViewModel;
using Inocrea.CodaBox.CodaApiClient.Helper;
using Newtonsoft.Json;
using CompteBancaire = Inocrea.CodaBox.ApiModel.CompteBancaire;

namespace Inocrea.CodaBox.CodaApiClient
{
    public partial class ApiClient
    {
        readonly ApiServerCoda _api = new ApiServerCoda();
       
        private List<Statements> repSta = new List<Statements>();

        private List<Transactions> repTra = new List<Transactions>();


        public async Task<List<StatementAccountViewModel>> GetStatements()
        {
          
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.CurrentCulture,
                "api/Statements"));


            repSta =  await  GetAsync<Statements>(requestUrl);
          
            

           
            return GetBusinessStatements
                (repSta);
        }

        public async Task<List<TransactionsAccountViewModel>> GetTransactions(int statementId)
        {

            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.CurrentCulture,
                "api/transactions/getTransactionsByStatement/"+ statementId));


            repTra = await GetAsync<Transactions>(requestUrl);




            return GetBusinessTransactions
                (repTra);
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
     
        private List<StatementAccountViewModel> GetBusinessStatements(List<Statements> statementsList )

        {
            List<StatementAccountViewModel> listStateAccountViewModels=new List<StatementAccountViewModel>();
            foreach (var statement in statementsList)
            {
                StatementAccountViewModel sta = new StatementAccountViewModel();
                CompteBancaire cp=new CompteBancaire();
                cp = GetCompte(statement.CompteBancaireId).Result;
                statement.CompteBancaire = cp;
                sta.StatementId = statement.StatementId;
                sta.Date = statement.Date;
                sta.InitialBalance = statement.InitialBalance;
                sta.NewBalance = statement.NewBalance;
                sta.Iban = statement.CompteBancaire.Iban;
                sta.IdentificationNumber = statement.CompteBancaire.IdentificationNumber;
                sta.Bic = statement.CompteBancaire.Bic;
                sta.InformationalMessage = statement.InformationalMessage;
                sta.CurrencyCode = statement.CompteBancaire.CurrencyCode;
               
                listStateAccountViewModels.Add(sta);

            }

            return listStateAccountViewModels;
        }



        private List<TransactionsAccountViewModel> GetBusinessTransactions(List<Transactions> transactionsList)

        {
            List<TransactionsAccountViewModel> listStateAccountViewModels = new List<TransactionsAccountViewModel>();
            foreach (var transactions in transactionsList)
            {
                TransactionsAccountViewModel tra = new TransactionsAccountViewModel();
                CompteBancaire cp = new CompteBancaire();
                cp = GetCompte(transactions.CompteBancaireId).Result;
                transactions.CompteBancaire = cp;
                tra.Amount = transactions.Amount;
                tra.StructuredMessage = transactions.StructuredMessage;
                tra.ValueDate = transactions.ValueDate;
                tra.TransactionDate = transactions.TransactionDate;
                tra.Iban = transactions.CompteBancaire.Iban;
                tra.IdentificationNumber = transactions.CompteBancaire.IdentificationNumber;
                tra.Bic = transactions.CompteBancaire.Bic;
                
                tra.CurrencyCode = transactions.CompteBancaire.CurrencyCode;

                listStateAccountViewModels.Add(tra);

            }

            return listStateAccountViewModels;
        }


    }
}
