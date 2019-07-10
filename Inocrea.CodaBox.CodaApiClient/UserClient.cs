using Inocrea.CodaBox.ApiModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inocrea.CodaBox.CodaApiClient
{
    public partial class ApiClient
    {
        public async Task<List<InvoiceModel>> GetInvoice()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                ""));
            return await GetAsync<List<InvoiceModel>>(requestUrl);
        }

        
    }
}
