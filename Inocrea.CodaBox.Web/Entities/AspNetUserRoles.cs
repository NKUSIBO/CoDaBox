using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inocrea.CodaBox.Web.Entities
{
    public partial class AspNetUserRoles
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
       
        public string RoleId { get; set; }

        public AspNetRoles Role { get; set; }
        public AspNetUsers User { get; set; }
    }
}
