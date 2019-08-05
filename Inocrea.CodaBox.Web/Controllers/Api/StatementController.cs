using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Inocrea.CodaBox.ApiModel;
using Inocrea.CodaBox.ApiModel.ViewModel;
using Inocrea.CodaBox.Web.Factory;
using Inocrea.CodaBox.Web.Helper;
using Inocrea.CodaBox.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Inocrea.CodaBox.Web.Controllers.Api
{
    [Route("Api/Statements")]
    public class StatementController : Controller
    {
        private readonly IOptions<SettingsModels> _appSettings;

        public StatementController(IOptions<SettingsModels> app)
        {
            _appSettings = app;
            AppSettings.ApiUrl = "Https://" + _appSettings.Value.WebApiBaseUrl;
        }

        //public IQueryable<Transactions> filteringTransaction()
        //{
        //    string searchText = string.Empty;

        //    Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };
        //    var requestFormData = Request.Form;
        //    tempOrder = new[] { "" };
        //    if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
        //    {
        //        var lstElements = new List<Transactions>();
        //        var results = lstElements.AsQueryable();
        //        var columnIndex = requestFormData["order[0][column]"].ToString();
        //        tempOrder = new[] { "" };
        //        if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
        //        {
        //            var columName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

                    
        //                var prop = GetProperty(columName);
                        
        //                    return lstElements
        //                        .Where(x => x.Name.ToLower().Contains(searchText.ToLower())
        //                                    || x.StructuredMessage.ToLower().Contains(searchText.ToLower()) || x.Message.ToLower().Contains(searchText.ToLower()))
                                
                        

        //                    .Where(
        //                        x => x.Name.ToLower().Contains(searchText.ToLower())
        //                             || x.StructuredMessage.ToLower().Contains(searchText.ToLower()) || x.Message.ToLower().Contains(searchText.ToLower()))
                          
                    

        //            return results;
        //        }
        //    }

        //    return null;
        //}
        [HttpPost]
        public async Task<IActionResult> LoadTransaction()
        {
            var requestFormData = Request.Form;
            List<StatementAccountViewModel> data = await ApiClientFactory.Instance.GetStatements();
            

            try
            {
                var listData = ProcessCollection(data, requestFormData);
                int transFiltered = GetTotalRecordsFiltered(requestFormData, data, listData);
                dynamic response = new
                {
                    data = listData,
                    draw = requestFormData["draw"],
                    recordsFiltered =transFiltered,
                    recordsTotal = data.Count
                };
                return Ok(response);

                //return Json(response);
            }
            catch (Exception )
            {
               
 
                return BadRequest();
            }



        }
        /// <summary>
        /// Process a list of items according to Form data parameters
        /// </summary>
        /// <param name="lstData">list of elements</param>
        /// <param name="requestFormData">collection of form data sent from client side</param>
        /// <returns>list of items processed</returns>
        private List<StatementAccountViewModel> ProcessCollection(List<StatementAccountViewModel> lstElements, Microsoft.AspNetCore.Http.IFormCollection requestFormData)
        {
            string searchText = string.Empty;
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };
            if (requestFormData.TryGetValue("search[value]", out tempOrder))
            {
                searchText = requestFormData["search[value]"].ToString();
            }
            tempOrder = new[] { "" };
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());

            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

                    if (pageSize > 0)
                    {
                        var prop = GetProperty(columName);
                        if (sortDirection == "asc")
                        {
                            return lstElements
                                .Where(x => x.Date.ToString(CultureInfo.CurrentCulture).ToLower().Contains(searchText.ToLower())
                                       || x.InitialBalance.ToString(CultureInfo.CurrentCulture).ToLower().Contains(searchText.ToLower())||x.NewBalance.ToString() .ToLower().Contains(searchText.ToLower()))
                                .Skip(skip)
                                .Take(pageSize)
                                .OrderBy(prop.GetValue).ToList();
                        }

                        return lstElements
                            .Where(x => x.Date.ToString(CultureInfo.CurrentCulture).ToLower().Contains(searchText.ToLower())
                                        || x.InitialBalance.ToString(CultureInfo.CurrentCulture).ToLower().Contains(searchText.ToLower()) || x.NewBalance.ToString().ToLower().Contains(searchText.ToLower()))
                            .Skip(skip)
                            .Take(pageSize)
                            .OrderByDescending(prop.GetValue).ToList();
                    }

                    return lstElements;
                }
            }
            return null;
        }
        /// <summary>
        /// Gets Total number of records filtered in a collection
        /// </summary>        
        /// <param name="requestFormData">collection of form data sent from client side</param>
        /// <param name="lstElements">list of elements</param>
        /// <param name="listProcessedItems">list filtered elements</param>
        /// <returns>Total records filtered</returns>
        private int GetTotalRecordsFiltered(IFormCollection requestFormData, List<StatementAccountViewModel> lstItems, List<StatementAccountViewModel> listProcessedItems)
        {
            var recFiltered = 0;
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };
            if (requestFormData.TryGetValue("search[value]", out tempOrder))
            {
                if (string.IsNullOrEmpty(requestFormData["search[value]"].ToString().Trim()))
                {
                    recFiltered = lstItems.Count;
                }
                else
                {
                    recFiltered = listProcessedItems.Count;
                }
            }
            return recFiltered;

        }

        //private object ProcessModuleCollection(List<Transactions> listData, IFormCollection requestFormData)
        //{
        //    string searchText = string.Empty;
        //    Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };
        //    if (requestFormData.TryGetValue("search[value]", out tempOrder))
        //    {
        //        searchText = requestFormData["search[value]"].ToString();
        //    }
        //    tempOrder = new[] { "" };
        //    var skip = Convert.ToInt32(requestFormData["start"].ToString());
        //    var pageSize = Convert.ToInt32(requestFormData["length"].ToString());

        //    if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
        //    {
        //        var columnIndex = requestFormData["order[0][column]"].ToString();
        //        var sortDirection = requestFormData["order[0][dir]"].ToString();
        //        tempOrder = new[] { "" };
        //        if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
        //        {
        //            var columName = requestFormData[$"columns[{columnIndex}][data]"].ToString();

        //            if (pageSize > 0)
        //            {
        //                var prop = GetProperty(columName);
        //                if (sortDirection == "asc")
        //                {
        //                    return listData
        //                        .Where(x => x.Name.ToLower().Contains(searchText.ToLower())
        //                               || x.Description.ToLower().Contains(searchText.ToLower()))
        //                        .Skip(skip)
        //                        .Take(pageSize)
        //                        .OrderBy(prop.GetValue).ToList();
        //                }
        //                else
        //                    return listData
        //                        .Where(
        //                                x => x.Name.ToLower().Contains(searchText.ToLower())
        //                                || x.Description.ToLower().Contains(searchText.ToLower()))
        //                        .Skip(skip)
        //                        .Take(pageSize)
        //                        .OrderByDescending(prop.GetValue).ToList();
        //            }
        //            else
        //                return listData;
        //        }
        //    }
        //    return null;
        //}

        private PropertyInfo GetProperty(string columnName)
        {
            var properties = typeof(Transactions).GetProperties();
            PropertyInfo prop = null;
            foreach (var item in properties)
            {
                if (item.Name.ToLower().Equals(columnName.ToLower()))
                {
                    prop = item;
                    break;

                }
            }

            return prop;
        }
    }
}