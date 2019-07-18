using System;
using System.Collections.Generic;
using Inocrea.CodaBox.ApiModel;

namespace Inocrea.CodaBox.ApiModel
{
    public partial class Iban
    {
        public Iban()
        {
            Statements = new HashSet<Statements>();
        }

        public int Id { get; set; }
        public string Iban1 { get; set; }
        public int? CompanyId { get; set; }

        public Company Company { get; set; }
        public ICollection<Statements> Statements { get; set; }
    }
}
