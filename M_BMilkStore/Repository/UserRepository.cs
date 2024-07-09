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
    public class UserRepository:IUserRepository
    {
        private readonly UserDAO _userDAO;
        public UserRepository(UserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        public async Task<bool> AddCustomerAsync(User user)
        {
            return await _userDAO.AddCustomersync(user);
        }

        public async Task<User> GetAnUserByEmail(string email)
        {
            return await _userDAO.GetUserByEmail(email);
        }
        public async Task<User> GetUserByID(int id) => await _userDAO.GetUserByID(id);
        public async Task<bool> UpdateUserAsync(User updatedUser) => await _userDAO.UpdateUserAsync(updatedUser);
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userDAO.GetUsersAsync();
        }

        public async Task CreateUserAsync(User user)
        {
             await _userDAO.AddUserAsync(user);
        }

        public Task DeleteUser(int userid)
        {
            return _userDAO.DeleteUserAsync(userid);
        }
    }
}
