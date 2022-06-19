using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Models;

namespace HealthCare.Domain.Interfaces 
{
    public interface ICredentialsService : IService<CredentialsDomainModel> 
    {
        public Task<CredentialsDomainModel> GetCredentialsByUsernameAndPassword(LoginDTO dto);
        public Task<IEnumerable<CredentialsDomainModel>> ReadAll();
    }
}
