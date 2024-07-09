using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> GetAnUserByEmail(string email);
        public Task<bool> AddCustomerAsync(User user);
        public Task<User> GetUserByID(int id);
        public Task<bool> UpdateUserAsync(User updatedUser);
        public Task<List<User>> GetAllUsersAsync();
        public Task CreateUserAsync(User user);
        public Task DeleteUser(int userid);
    }
}
