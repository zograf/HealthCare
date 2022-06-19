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
    public interface IManagerRepository : IRepository<Manager>
    {
        public Task<Manager> GetById(decimal id);
    }
    public class ManagerRepository : IManagerRepository 
    {
        private readonly HealthCareContext _healthCareContext;

        public ManagerRepository(HealthCareContext healthCareContext) 
        {
            _healthCareContext = healthCareContext;
        }
        public async Task<IEnumerable<Manager>> GetAll() 
        {
            return await _healthCareContext.Managers
                .Include(x => x.Credentials)
                .ToListAsync();
        }
        public async Task<Manager> GetById(decimal id) 
        {
            return await _healthCareContext.Managers
                .Include(x => x.Credentials)
                .Where(x=>x.Id == id)
                .FirstOrDefaultAsync();
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }
    }
}
