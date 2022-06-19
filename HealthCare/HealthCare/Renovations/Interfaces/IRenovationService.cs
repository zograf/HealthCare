using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Interfaces
{
    public interface IRenovationService : IService<RenovationDomainModel>
    {
        public Task<IEnumerable<RenovationDomainModel>> GetAll();
        public Task ExecuteComplexRenovations();
        public Task<bool> IsAvaliable(Room room, CreateRenovationDTO renovationToCheck);
    }
}
