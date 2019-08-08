using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Inocrea.CodaBox.ApiServer.Entities
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    [Table("CodaIdentities")]
    public class CodaIdentity
    {
        public int CodaIdentityId { get; set; }
        public string XCompany { get; set; }
        public string Login { get; set; }
        public string Pwd { get; set; }
    }
}