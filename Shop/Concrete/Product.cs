using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Concrete
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public double UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public override string ToString()
        {
            return $"{ProductId} {ProductName} {CategoryId} {UnitPrice} {UnitsInStock}";
        }
    }
}
