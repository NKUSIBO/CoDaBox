using Inocrea.CodaBox.ApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using System.Threading.Tasks;
using Inocrea.CodaBox.ApiModel.Models;
using Inocrea.CodaBox.ApiModel.ViewModel;
using Inocrea.CodaBox.CodaApiClient.Helper;
using Newtonsoft.Json;

using CompteBancaire = Inocrea.CodaBox.ApiModel.Models.CompteBancaire;


namespace Inocrea.CodaBox.CodaApiClient
{
    public partial class ApiClient
    {
        readonly ApiServerCoda _api = new ApiServerCoda();
       
        private List<Statements> repSta = new List<Statements>();

        private List<Transactions> repTra = new List<Transactions>();

        public async Task<List<StatementAccountViewModel>> GetStatementsAccountVm()
        {
            await GetStatements();
            return await GetBusinessStatements
               (repSta);
        }
        public async Task<List<Statements>> GetStatements()
        {
          
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.CurrentCulture,
                "api/Statements"));


            repSta =  await  GetAsync<Statements>(requestUrl);



            return repSta;
            //return await GetBusinessStatementsAsync
            //    (repSta);
        }

        public async Task<List<TransactionsAccountViewModel>> GetTransactions(int statementId)
        {

            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.CurrentCulture,
                "api/transactions/getTransactionsByStatement/"+ statementId));


            repTra = await GetAsync<Transactions>(requestUrl);




            return GetBusinessTransactions
                (repTra);
        }


        public async Task<CompteBancaire> GetCompteBancaire(int compteId)
        {

            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.CurrentCulture,
                "api/CompteBancaires/" + compteId));


            var compte = await GetDetailAsync<CompteBancaire>(requestUrl);




            return compte;
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
     
        private async Task<List<StatementAccountViewModel>> GetBusinessStatements(List<Statements> statementsList )

        {
            List<StatementAccountViewModel> listStateAccountViewModels=new List<StatementAccountViewModel>();
            foreach (var statement in statementsList)
            {
                StatementAccountViewModel sta = new StatementAccountViewModel();
                CompteBancaire cp=new CompteBancaire();
                //cp = await GetCompteBancaire(statement.CompteBancaireId);
                //cp = GetCompte(statement.CompteBancaireId).Result;
                cp = statement.CompteBancaire;
                
                sta.StatementId = statement.StatementId;
                sta.Date = statement.Date;
                sta.InitialBalance = statement.InitialBalance;
                sta.NewBalance = statement.NewBalance;
                sta.Iban = cp.Iban;
                sta.IdentificationNumber = cp.IdentificationNumber;
                sta.Bic = cp.Bic;
                sta.InformationalMessage = statement.InformationalMessage;
                sta.CurrencyCode = cp.CurrencyCode;
               
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
                //cp = transactions.CompteBancaire;
                //cp = GetCompteBancaire(transactions.CompteBancaireId).Result;
                cp = transactions.CompteBancaire;
                tra.Amount = transactions.Amount;
                tra.Message = transactions.Message;
                tra.StructuredMessage = transactions.StructuredMessage;
                tra.ValueDate = transactions.ValueDate;
                tra.TransactionDate = transactions.TransactionDate;
                tra.Iban = cp.Iban;
                tra.IdentificationNumber = cp.IdentificationNumber;
                tra.Bic = cp.Bic;
                
                tra.CurrencyCode = cp.CurrencyCode;

                listStateAccountViewModels.Add(tra);

            }

            return listStateAccountViewModels;
        }
        public async Task<Message<RegisterModel>> SaveUser(RegisterModel model)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "api/Account/register"));
            return await PostAsync<RegisterModel>(requestUrl, model);
        }
        public async Task<Message<LoginModel>> LogUser(LoginModel model)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "api/Account/token"));
            return await PostAsync<LoginModel>(requestUrl, model);
        }
        public async Task<Message<LoginModel>> LogOut()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "api/Account/logout"));
            return await PostAsync<LoginModel>(requestUrl,null);
        }

    }
}
