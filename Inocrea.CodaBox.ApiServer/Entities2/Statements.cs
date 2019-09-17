using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Inocrea.CodaBox.ApiModel.Models;
using Newtonsoft.Json;

namespace Inocrea.CodaBox.ApiServer.Entities2
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public partial class Statements
    {
        public Statements()
        {
            Transactions = new HashSet<Transactions>();
        }
        [Key]
        public int StatementId { get; set; }
        [ForeignKey("CompteBancaire")]
        public int CompteBancaireId { get; set; }
        public double InitialBalance { get; set; }
        public double NewBalance { get; set; }
        public string InformationalMessage { get; set; }
        public DateTime Date { get; set; }
        public int? IdCompteBancaire { get; set; }

        public virtual CompteBancaire CompteBancaire { get; set; }
        public virtual ICollection<Transactions> Transactions { get; set; }
    }
}
