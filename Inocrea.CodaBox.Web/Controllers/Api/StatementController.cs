using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Inocrea.CodaBox.ApiModel;
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
        [HttpPost]
        public async Task<IActionResult> LoadTransaction()
        {
            var requestFormData = Request.Form;

            List<Transactions> data = await ApiClientFactory.Instance.GetInvoice();

            try
            {
                var listData = ProcessModuleCollection(data, requestFormData);
               
                dynamic response = new
                {
                    data = listData,
                    draw = requestFormData["draw"],
                    recordsFiltered = data.Count,
                    recordsTotal = data.Count
                };
                return Ok(response);

                //return Json(response);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }



        }

        private object ProcessModuleCollection(List<Transactions> listData, IFormCollection requestFormData)
        {
            var skip = Convert.ToInt32(requestFormData["start"].ToString());
            var pageSize = Convert.ToInt32(requestFormData["length"].ToString());
            Microsoft.Extensions.Primitives.StringValues tempOrder = new[] { "" };
            if (requestFormData.TryGetValue("order[0][column]", out tempOrder))
            {
                var columnIndex = requestFormData["order[0][column]"].ToString();
                var sortDirection = requestFormData["order[0][dir]"].ToString();
                tempOrder = new[] { "" };
                if (requestFormData.TryGetValue($"columns[{columnIndex}][data]", out tempOrder))
                {
                    var columnName = requestFormData[$"columns[{columnIndex}][data]"].ToString();
                    if (pageSize > 0)
                    {
                        var prop = getProperty(columnName);
                        if (sortDirection == "asc")
                        {
                            return listData.OrderBy(prop.GetValue).Skip(skip).Take(pageSize).ToList();
                        }

                        return listData.OrderByDescending(prop.GetValue).Skip(skip).Take(pageSize).ToList();


                    }

                    return listData;




                }
            }

            return null;
        }

        private PropertyInfo getProperty(string columnName)
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