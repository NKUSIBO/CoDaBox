using Inocrea.CodaBox.ApiModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        //public async Task<List<StatementAccountViewModel>> GetStatementsAccountVm()
        //{
        //    await GetStatements();
        //    return 
        //       repSta;
        //}
        public async Task<List<StatementAccountViewModel>> GetStatements()
        {
          
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.CurrentCulture,
                "api/Statements"));


            repSta =  await  GetAsync<Statements>(requestUrl);



            
            return  GetBusinessStatements(repSta);
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

        private List<StatementAccountViewModel> GetBusinessStatements(List<Statements> statementsList)

        {
            List<StatementAccountViewModel> listStateAccountViewModels = new List<StatementAccountViewModel>();
            foreach (var statement in statementsList)
            {
                StatementAccountViewModel sta = new StatementAccountViewModel();
                CompteBancaire cp = new CompteBancaire();
                cp = GetCompteBancaire(statement.CompteBancaireId).Result;
                //cp = GetCompte(statement.CompteBancaireId).Result;
                statement.CompteBancaire=cp;

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

                //cp = transactions.CompteBancaire;
                CompteBancaire cp = GetCompteBancaire(transactions.CompteBancaireId).Result;
                transactions.CompteBancaire=cp;
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

        public async Task<List<TransactionsAccountViewModel>> GetTransactionsByDateAsync(int statementId, DateTime? datepickerStart, DateTime? datepickerEnd)
        {
            string startDate = null;
            string endDate = null;
            List<TransactionsAccountViewModel> returnList =new List<TransactionsAccountViewModel>();
            if (datepickerStart != null)
            {
                 startDate = datepickerStart.Value.Date.ToString("MM/dd/yyyy");
                 startDate = startDate.Replace("/", "-");
                 startDate = startDate.Replace(" 00:00:00", "%2000%3A00%3A00");
            }

            if (datepickerEnd != null)
            {
                 endDate = datepickerEnd.Value.Date.ToString("MM/dd/yyyy");
                 endDate = endDate.Replace("/", "-");
                 endDate = endDate.Replace(" 00:00:00", "%2000%3A00%3A00");

            }

            string parameters = statementId+"/"+ startDate +"/"+ endDate;
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "api/transactions/GetTransactionsByDate/" + parameters));

            repTra = await GetAsync<Transactions>(requestUrl);
         
           
            return   GetBusinessTransactions(repTra); ;
        }

        public async Task<List<TransactionsAccountViewModel>> GetTransactionsByDateIban(string Iban, DateTime? datepickerStart, DateTime? datepickerEnd)
        {
            string startDate = null;
            string endDate = null;
            List<TransactionsAccountViewModel> returnList = new List<TransactionsAccountViewModel>();
            if (datepickerStart != null)
            {
                startDate = datepickerStart.Value.Date.ToString("MM/dd/yyyy");
                startDate = startDate.Replace("/", "-");
                startDate = startDate.Replace(" 00:00:00", "%2000%3A00%3A00");
            }

            if (datepickerEnd != null)
            {
                endDate = datepickerEnd.Value.Date.ToString("MM/dd/yyyy");
                endDate = endDate.Replace("/", "-");
                endDate = endDate.Replace(" 00:00:00", "%2000%3A00%3A00");

            }

            string parameters = Iban + "/" + startDate + "/" + endDate;
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "api/Statements/" + parameters));

            repTra = await GetAsync<Transactions>(requestUrl);


            return GetBusinessTransactions(repTra); ;
        }


    }
}
