﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Inocrea.CodaBox.ApiModel.Models
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CompteBancaire
    {
        public CompteBancaire()
        {
            Statements = new HashSet<Statements>();
            Transactions = new HashSet<Transactions>();
        }

        public int CompteBancaireId { get; set; }
        public string Iban { get; set; }
        public string Bic { get; set; }
        public string IdentificationNumber { get; set; }
        public string CurrencyCode { get; set; }
        public int? CompanyId { get; set; }

        public virtual Company Company { get; set; }
        public virtual ICollection<Statements> Statements { get; set; }
        public virtual ICollection<Transactions> Transactions { get; set; }

        public override string ToString()
        {
            return Iban;
        }
    }
}