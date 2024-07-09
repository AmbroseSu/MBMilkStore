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
        public async Task<bool> AddCustomersync(User user)
        {
            try
            {
                user.RoleId = 2;
                user.Status = true;
                user.IsDeleted = false;
                _dbContext.Users.Add(user);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync("Add Customer fail");
                return false;
            }
        }

        public async Task<User> GetUserByID(int id)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(users => users.UserId == id);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Getting User By ID Failed: " + ex.Message);
            }
        }

        public async Task<bool> UpdateUserAsync(User updatedUser)
        {
            try
            {
                var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == updatedUser.UserId);
                if (existingUser != null)
                {
                    existingUser.LastName = updatedUser.LastName;
                    existingUser.FirstName = updatedUser.FirstName;
                    existingUser.Address = updatedUser.Address;
                    existingUser.PhoneNumber = updatedUser.PhoneNumber;
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Updateing User Failed: " + ex.Message);
            }
        }
        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                var users = await _dbContext.Users.Include(u => u.UserRole).ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Get List of user fail");
                return null;
            }
        }
        public async Task AddUserAsync(User user)
        {
            try
            {
                 _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Create user fail");
            }
        }
       public async Task DeleteUserAsync(int id)
        {
            try
            {
                var user = await GetUserByID(id);
                user.IsDeleted = true;
                _dbContext.Update(user);
                await _dbContext.SaveChangesAsync();
            }catch(Exception e)
            {
                await Console.Out.WriteLineAsync("Delete user fail");
            }
        }

    }
}
