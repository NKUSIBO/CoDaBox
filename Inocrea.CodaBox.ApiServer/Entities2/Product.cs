using System;
using System.Collections.Generic;

namespace Inocrea.CodaBox.ApiServer.Entities2
{
    public partial class Product
    {
        public Product()
        {
            OrderLine = new HashSet<OrderLine>();
        }

        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }

        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}
