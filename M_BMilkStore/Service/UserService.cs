using BussinessObject;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> CreateCustomerAsync(User user)
        {
            var existingUser = await _repository.GetAnUserByEmail(user.Email);
            if (existingUser != null)
            {
                return false;
            }

            await _repository.AddCustomerAsync(user);
            return true;
        }

        public async Task<User> GetAnUserByEmail(string email)
        {
            return await _repository.GetAnUserByEmail(email);
        }

        public async Task<User> GetUserByID(int id) => await _repository.GetUserByID(id);
        public async Task<bool> UpdateUserAsync(User updatedUser) => await _repository.UpdateUserAsync(updatedUser);
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _repository.GetAllUsersAsync();
        }

        public async Task CreateUserAsync(User user)
        {
             await _repository.CreateUserAsync(user);
        }

        public Task DeleteUserAsync(int userid)
        {
            return _repository.DeleteUser(userid);
        }
    }
}
