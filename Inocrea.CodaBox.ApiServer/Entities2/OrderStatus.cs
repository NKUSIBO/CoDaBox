using System;
using System.Collections.Generic;

namespace Inocrea.CodaBox.ApiServer.Entities2
{
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            Order = new HashSet<Order>();
        }

        public int OrderStatusId { get; set; }
        public string OrderStatusName { get; set; }
        public short OrderStatusProgress { get; set; }

        public virtual ICollection<Order> Order { get; set; }
    }
}
