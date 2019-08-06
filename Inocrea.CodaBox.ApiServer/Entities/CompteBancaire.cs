using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Inocrea.CodaBox.ApiServer.Entities
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CompteBancaire
    {
        public CompteBancaire()
        {
            Statements = new HashSet<Statements>();
            Transactions = new HashSet<Transactions>();
        }

        [Key]
        public int Id { get; set; }
        public string Iban { get; set; }
        public string Bic { get; set; }
        public string IdentificationNumber { get; set; }
        public string CurrencyCode { get; set; }

        [JsonIgnore]
        public virtual ICollection<Statements> Statements { get; set; }
        [JsonIgnore]
        public virtual ICollection<Transactions> Transactions { get; set; }
    }
}