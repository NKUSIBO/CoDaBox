using System;
using System.Collections.Generic;

namespace Inocrea.CodaBox.Dal.Models
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
