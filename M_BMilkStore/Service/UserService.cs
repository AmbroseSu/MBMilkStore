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

        public async Task<bool> CreateUserAsync(User user)
        {
            var existingUser = await _repository.GetAnUserByEmail(user.Email);
            if (existingUser != null)
            {
                return false;
            }

            await _repository.AddUserAsync(user);
            return true;
        }

        public async Task<User> GetAnUserByEmail(string email)
        {
            return await _repository.GetAnUserByEmail(email);
        }
    }
}
