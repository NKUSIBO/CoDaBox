using System;
using System.Collections.Generic;

namespace Inocrea.CodaBox.Web.Entities
{
    public partial class CodaIdentities
    {
        public int CodaIdentityId { get; set; }
        public string XCompany { get; set; }
        public string Login { get; set; }
        public string Pwd { get; set; }
    }
}
