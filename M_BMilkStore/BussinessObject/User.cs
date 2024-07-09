using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObject
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string? LastName {  get; set; }
        public string? FirstName {  get; set; }
        public string? Address {  get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? Status { get; set; }
        public bool? IsDeleted { get; set; }
        public int RoleId { get; set; }
        public virtual UserRole? UserRole { get; set; }
        public virtual ICollection<Order>? ListOrder { get; set; }
        public virtual ICollection<UserVoucher>? UserVouchers { get; set; }
    }
}
