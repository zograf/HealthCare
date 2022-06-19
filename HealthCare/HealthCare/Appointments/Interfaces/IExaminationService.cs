using HealthCare.Domain.DTOs;
using HealthCare.Domain.Models;

namespace HealthCare.Domain.Interfaces;

public interface IExaminationService : IService<ExaminationDomainModel> 
{
    public Task<ExaminationDomainModel> Delete(DeleteExaminationDTO dto);
    public Task<ExaminationDomainModel> Create(CUExaminationDTO dto);
    public Task<ExaminationDomainModel> Update(CUExaminationDTO dto);
    public Task<IEnumerable<ExaminationDomainModel>> ReadAll();

}