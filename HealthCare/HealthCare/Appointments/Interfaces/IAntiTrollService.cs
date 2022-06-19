using HealthCare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Interfaces 
{
    public interface IAntiTrollService : IService<AntiTrollDomainModel> 
    {
        public Task<IEnumerable<AntiTrollDomainModel>> GetByPatientId(decimal patientId);
        public Task<bool> AntiTrollCheck(decimal patientId, bool isCreate);
        public void WriteToAntiTroll(decimal patientId, string state);
    }
}
