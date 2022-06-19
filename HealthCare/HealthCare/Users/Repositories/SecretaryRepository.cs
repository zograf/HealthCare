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
    public interface ISecretaryRepository : IRepository<Secretary>
    {
        public Task<Secretary> GetById(decimal id);
    }
    public class SecretaryRepository : ISecretaryRepository 
    {
        private readonly HealthCareContext _healthCareContext;

        public SecretaryRepository(HealthCareContext healthCareContext) 
        {
            _healthCareContext = healthCareContext;
        }
        public async Task<IEnumerable<Secretary>> GetAll() 
        {
            return await _healthCareContext.Secretaries
                .Include(x => x.Credentials)
                .ToListAsync();
        }
        
        public async Task<Secretary> GetById(decimal id)
        {
            return await _healthCareContext.Secretaries
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
