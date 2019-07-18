using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inocrea.CodaBox.Web.Entities
{
    public partial class AspNetUserLogins
    {
        [Key]
        public string LoginProvider { get; set; }
        [Key]
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
        public string UserId { get; set; }

        public AspNetUsers User { get; set; }
    }
}
