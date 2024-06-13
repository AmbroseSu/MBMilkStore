using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject
{
    public class Voucher
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int VoucherId { get; set; }
        public string VoucherName { get; set; }
        public string VoucherValue { get; set;}
        public virtual ICollection<Order>? ListOrders { get; set; }
    }
}
