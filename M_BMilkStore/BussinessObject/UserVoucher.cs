using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObject
{
    public class UserVoucher
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int UserVoucherId { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int VoucherId { get; set; }
        public virtual Voucher Voucher { get; set; }

        public DateTime RedemptionDate { get; set; }
    }
}
