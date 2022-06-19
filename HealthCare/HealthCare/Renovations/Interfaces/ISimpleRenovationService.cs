using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Models;
using HealthCareAPI.Renovations.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Interfaces
{
    public interface ISimpleRenovationService : IService<SimpleRenovationDomainModel>
    {
        public Task<bool> validateSimpleRenovation(CreateSimpleRenovationDTO renovation);
        public Task<SimpleRenovationDomainModel> Create(CreateSimpleRenovationDTO dto);
        public SimpleRenovationDomainModel ParseToModel(SimpleRenovation simpleRenovation);
        public IEnumerable<SimpleRenovationDomainModel> ParseToModel(IEnumerable<SimpleRenovation> renovations);
    }
}
