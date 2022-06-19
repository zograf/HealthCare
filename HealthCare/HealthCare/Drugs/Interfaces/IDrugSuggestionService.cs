using HealthCare.Domain.DTOs;
using HealthCare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Interfaces
{
    public interface IDrugSuggestionService : IService<DrugSuggestionDomainModel>
    {
        public Task<IEnumerable<DrugSuggestionDomainModel>> GetAll();
        public Task<DrugSuggestionDomainModel> Create(DTOs.DrugDTO drugDTO);
        public Task<DrugSuggestionDomainModel> Delete(decimal drugSuggestionId);
        public Task<IEnumerable<DrugSuggestionDomainModel>> GetPending();
        public Task<IEnumerable<DrugSuggestionDomainModel>> GetRejected();
        public Task<DrugSuggestionDomainModel> Approve(decimal drugSuggestionId);
        public Task<DrugSuggestionDomainModel> Revision(decimal drugSuggestionId, string comment);
        public Task<DrugSuggestionDomainModel> Reject(decimal drugSuggestionId, string comment);
        public Task<DrugDomainModel> Update(DrugDTO dto);
    }
}
