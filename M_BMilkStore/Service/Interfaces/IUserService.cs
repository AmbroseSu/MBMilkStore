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
        public Task<bool> CreateUserAsync(User user);
    }
}
