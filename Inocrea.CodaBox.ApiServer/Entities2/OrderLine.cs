using System;
using System.Collections.Generic;

namespace Inocrea.CodaBox.ApiServer.Entities2
{
    public partial class OrderLine
    {
        public int OrderLineId { get; set; }
        public int OrderLineQuantity { get; set; }
        public decimal OrderLinePrice { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
