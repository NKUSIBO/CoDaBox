using System;
using System.Collections.Generic;

namespace Inocrea.CodaBox.ApiServer.Entities2
{
    public partial class Order
    {
        public Order()
        {
            OrderLine = new HashSet<OrderLine>();
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime FiscalDate { get; set; }
        public string OrderReference { get; set; }
        public int OrderStatusId { get; set; }
        public int CompanyId { get; set; }
        public string ContactId { get; set; }

        public virtual Company Company { get; set; }
        public virtual AspNetUsers Contact { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}
