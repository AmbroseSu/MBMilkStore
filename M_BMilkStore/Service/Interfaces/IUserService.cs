using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IUserService
    {
        public Task<User> GetAnUserByEmail(string email);
        public Task<bool> CreateCustomerAsync(User user);
        public Task<User> GetUserByID(int id);
        public Task<bool> UpdateUserAsync(User updatedUser);
        public Task<List<User>> GetAllUsersAsync();
        public Task CreateUserAsync(User user);
        public Task DeleteUserAsync(int userid);
    }
}
