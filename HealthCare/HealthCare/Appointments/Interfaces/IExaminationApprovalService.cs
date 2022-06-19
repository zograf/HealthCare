using HealthCare.Domain.Models;

namespace HealthCare.Domain.Interfaces;

public interface IExaminationApprovalService : IService<ExaminationApprovalDomainModel> 
{
    public Task<ExaminationApprovalDomainModel> Reject(decimal id);
    public Task<ExaminationApprovalDomainModel> Approve(decimal id);
    public Task<IEnumerable<ExaminationApprovalDomainModel>> ReadAll();
}