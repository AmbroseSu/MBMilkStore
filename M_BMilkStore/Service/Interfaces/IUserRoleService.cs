using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IUserRoleService
    {
        public Task<List<UserRole>> GetAllUserRolesAsync();
    }
}
