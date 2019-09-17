using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Inocrea.CodaBox.ApiModel
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    [Table("CodaIdentities")]
    public partial class CodaIdentities
    {
        public int CodaIdentityId { get; set; }
        public string XCompany { get; set; }
        public string Login { get; set; }
        public string Pwd { get; set; }
    }
}
