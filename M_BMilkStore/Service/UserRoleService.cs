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
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _repository;
        public UserRoleService(IUserRoleRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<UserRole>> GetAllUserRolesAsync()
        {
            return await _repository.GetAllUserRolesAsync();
        }
    }
}
