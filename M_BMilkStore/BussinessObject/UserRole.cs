using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject
{
    public class UserRole
    {
        
        [Key]
        public int UserRoleId {  get; set; }
        public string UserRoleName {  get; set; }
        public virtual ICollection<User>? ListUser { get; set; }
    }
}
