using BussinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DAO
{
    public class UserDAO
    {
        private readonly M_BMilkStoreDBContext _dbContext;
        public UserDAO(M_BMilkStoreDBContext dbContext) { _dbContext = dbContext; }

        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                var user = await _dbContext.Users.Include(u => u.UserRole).FirstOrDefaultAsync(x => x.Email.Equals(email));
                return user;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Get User By Email fail");
                return null;
            }
        }
        public async Task<bool> AddUserAsync(User user)
        {
            user.RoleId = 2;
            user.Status = true;
            user.IsDeleted = false;
            _dbContext.Users.Add(user);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
