using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool Status { get; set; }
        public float OrderTotalAmount { get; set; }
        public int UserId { get; set; }
        public int VoucherId { get; set; }
        public virtual User? User { get; set; }
        public virtual Voucher? Voucher { get; set; }
        public virtual ICollection<OrderDetail>? ListOrderDetail { get; set; }
    }
}
