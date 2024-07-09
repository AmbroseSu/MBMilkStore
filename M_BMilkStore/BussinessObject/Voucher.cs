using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObject
{
    public class Voucher
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int VoucherId { get; set; }

        [Required(ErrorMessage = "Voucher name is required.")]
        public string VoucherName { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Voucher value must be a positive number.")]
        public decimal VoucherValue { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Minimum price must be a positive number.")]
        public decimal MinimumPrice { get; set; }

        [Required(ErrorMessage = "Creation date is required.")]
        public DateTime CreationDate { get; set; }

        [Required(ErrorMessage = "Expiry date is required.")]
        public DateTime ExpiryDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Order>? ListOrders { get; set; }
        public virtual ICollection<UserVoucher>? UserVouchers { get; set; }
    }
}
