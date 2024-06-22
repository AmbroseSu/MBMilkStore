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
        public int? UserId { get; set; } // Reference to the user who added the item
        public Product Product { get; set; }
        public int Quantity { get; set; } // Quantity of the product in the cart
    }
}
