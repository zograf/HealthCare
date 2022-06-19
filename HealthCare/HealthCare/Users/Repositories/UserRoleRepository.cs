using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Data.Context;
using HealthCare.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Repositories 
{
    public interface IUserRoleRepository : IRepository<UserRole> 
    {
        public Task<UserRole> GetById(decimal id);
        public Task<UserRole> GetByRoleName(string roleName);
    }
    public class UserRoleRepository : IUserRoleRepository 
    {
        private readonly HealthCareContext _healthCareContext;

        public UserRoleRepository(HealthCareContext healthCareContext) 
        {
            _healthCareContext = healthCareContext;
        }
        public async Task<IEnumerable<UserRole>> GetAll() {
            return await _healthCareContext.UserRoles.ToListAsync();
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }

        public async Task<UserRole> GetById(decimal id) {
            return await _healthCareContext.UserRoles.FirstAsync(x => x.Id == id);
        }
        public async Task<UserRole> GetByRoleName(string roleName) {
            return await _healthCareContext.UserRoles.FirstAsync(x => x.RoleName.Equals(roleName));
        }
    }
}
