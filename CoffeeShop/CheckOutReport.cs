using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoffeeShop
{
   public class CheckOutReport
    {
        public String ProID { get; set; }
        public String ProName { get; set; }
        public Decimal Price { get; set; }
        public String size { get; set; }
        public Int32 qty { get; set; }
        public Decimal Total { get; set; }
        public String Name { get; set; }
        public String UserID { get; set; }
    }
}
