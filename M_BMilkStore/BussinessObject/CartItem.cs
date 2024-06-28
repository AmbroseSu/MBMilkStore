using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject
{
    public class CartItem
    {
        public Product? Product { get; set; }
        public int Quantity { get; set; } // Quantity of the product in the cart
    }
}
