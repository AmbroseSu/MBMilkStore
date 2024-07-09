using BussinessObject;
using DataAccessLayer.DAO;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly UserRoleDAO _userRoleDAO;
        public UserRoleRepository(UserRoleDAO userRoleDAO)
        {
            _userRoleDAO = userRoleDAO;
        }
        public async Task<List<UserRole>> GetAllUserRolesAsync()
        {
            return await _userRoleDAO.GetUserRolesAsync();
        }
    }
}
