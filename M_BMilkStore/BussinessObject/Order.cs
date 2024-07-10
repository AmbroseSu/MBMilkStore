using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject
{
    public enum RefundStatus
    {
        None,
        Requested,
        Approved,
        Rejected,
    }

    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public bool Status { get; set; }
        public float OrderTotalAmount { get; set; }
        public int UserId { get; set; }
        public int? VoucherId { get; set; }
        public bool isDeleted { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public RefundStatus RefundStatus { get; set; } = RefundStatus.None;
        public DateTime? RefundRequestDate { get; set; }
        public string? RefundMessage { get; set; }
        public virtual User? User { get; set; }
        public virtual Voucher? Voucher { get; set; }
        public ICollection<OrderDetail> ListOrderDetail { get; set; } = new List<OrderDetail>();
    }
}
