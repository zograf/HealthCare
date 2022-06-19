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
    public interface IJoinRenovationService : IService<JoinRenovationDomainModel>
    {
        public Task<bool> validateJoinRenovation(CreateJoinRenovationDTO dto);
        public Task<JoinRenovationDomainModel> Create(CreateJoinRenovationDTO dto);
        public JoinRenovationDomainModel ParseToModel(JoinRenovation joinRenovation);
        public IEnumerable<JoinRenovationDomainModel> ParseToModel(IEnumerable<JoinRenovation> renovations);
    }
}
