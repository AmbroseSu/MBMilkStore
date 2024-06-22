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
        public Task<bool> AddUserAsync(User user);
    }
}
