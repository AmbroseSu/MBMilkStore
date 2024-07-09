using BussinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DAO
{
    public class UserRoleDAO
    {
        private readonly M_BMilkStoreDBContext _dbContext;
        public UserRoleDAO(M_BMilkStoreDBContext dbContext) { _dbContext = dbContext; }
        public async Task<List<UserRole>> GetUserRolesAsync()
        {
            try
            {
                var userroles = await _dbContext.UserRoles.ToListAsync();
                return userroles;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Get List of role fail");
                return null;
            }
        }
    }
}
