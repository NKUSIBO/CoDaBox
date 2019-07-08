using Inocrea.CodaBox.ApiModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inocrea.CodaBox.ApiClient
{
    public partial class ApiClient
    {
        public async Task<List<InvoiceModel>> GetInvoice()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "User/GetAllUsers"));
            return await GetAsync<List<InvoiceModel>>(requestUrl);
        }

        
    }
}
