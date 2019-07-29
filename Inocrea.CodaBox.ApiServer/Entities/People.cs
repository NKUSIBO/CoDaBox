using System;
using System.Collections.Generic;

namespace Inocrea.CodaBox.ApiServer.Entities
{
    public partial class People
    {
        public int PeopleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Genre { get; set; }
    }
}
