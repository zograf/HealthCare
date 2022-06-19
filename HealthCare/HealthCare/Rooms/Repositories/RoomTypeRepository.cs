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
    public interface IRoomTypeRepository : IRepository<RoomType>
    {
        public Task<RoomType> GetById(decimal roomTypeId);
    }
    public class RoomTypeRepository : IRoomTypeRepository 
    {
        private readonly HealthCareContext _healthCareContext;

        public RoomTypeRepository(HealthCareContext healthCareContext) 
        {
            _healthCareContext = healthCareContext;
        }
        public async Task<IEnumerable<RoomType>> GetAll() {
            return await _healthCareContext.RoomTypes.ToListAsync();
        }

        public void Save()
        {
            _healthCareContext.SaveChanges();
        }
        public async Task<RoomType> GetById(decimal roomTypeId)
        {
            return await _healthCareContext.RoomTypes.FirstOrDefaultAsync(x => x.Id == roomTypeId);
        }
    }
}
