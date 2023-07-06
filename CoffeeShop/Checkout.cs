using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoffeeShop
{
   public class Checkout
    {
        public String InvID { get; set; }
        public DateTime InvDate { get; set; }
        public TimeSpan InvTime { get; set; }
        public Decimal Amount { get; set; }
        public Decimal discount { get; set; }
        public Decimal payment { get; set; }
        public DateTime createdate { get; set; }
        public String UserID { get; set; }

    }
}
