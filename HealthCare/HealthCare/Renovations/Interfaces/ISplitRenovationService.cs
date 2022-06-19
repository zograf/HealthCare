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
    public interface ISplitRenovationService : IService<SplitRenovationDomainModel>
    {
        public Task<bool> validateSplitRenovation(CreateSplitRenovationDTO dto);
        public Task<SplitRenovationDomainModel> Create(CreateSplitRenovationDTO dto);
        public SplitRenovationDomainModel ParseToModel(SplitRenovation splitRenovation);
        public IEnumerable<SplitRenovationDomainModel> ParseToModel(IEnumerable<SplitRenovation> renovations);
    }
}
