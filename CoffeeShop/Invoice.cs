using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoffeeShop
{
    class Invoice
    {
        public String InvID { get; set; }
        public DateTime InvDate { get; set; }
        public TimeSpan InvTime { get; set; }
        public Decimal Amount { get; set; }
        public Decimal discount { get; set; }
        public Decimal payment { get; set; }
        public String ProName { get; set; }
        public Decimal dis { get; set; }
        public String size { get; set; }
        public Int32 Qty { get; set; }
        public Decimal Price { get; set; }
        public Decimal Total { get; set; }
        public String UserID { get; set; }
        public String waiting { get; set; }
    }
}
