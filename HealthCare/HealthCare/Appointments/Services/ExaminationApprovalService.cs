using HealthCare.Data.Entities;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Repositories;

namespace HealthCare.Domain.Services;

public class ExaminationApprovalService : IExaminationApprovalService
{
    private IExaminationApprovalRepository _examinationApprovalRepository;
    private IExaminationRepository _examinationRepository;

    public ExaminationApprovalService(IExaminationApprovalRepository examinationApprovalRepository, 
                                      IExaminationRepository examinationRepository) 
    {
        _examinationApprovalRepository = examinationApprovalRepository;
        _examinationRepository = examinationRepository;
    }

    // Async awaits info from database
    // GetAll is the equivalent of SELECT *
    public async Task<IEnumerable<ExaminationApprovalDomainModel>> GetAll()
    {
        IEnumerable<ExaminationApproval> data = await _examinationApprovalRepository.GetAll();
        if (data == null)
            return new List<ExaminationApprovalDomainModel>();

        List<ExaminationApprovalDomainModel> results = new List<ExaminationApprovalDomainModel>();
        ExaminationApprovalDomainModel examinationApprovalModel;
        foreach (ExaminationApproval item in data)
        {
            results.Add(ParseToModel(item));
        }

        return results;
    }
    
    public static ExaminationApproval ParseFromModel(ExaminationApprovalDomainModel examinationApprovalModel)
    {
        ExaminationApproval examinationApproval = new ExaminationApproval
        {
            Id = examinationApprovalModel.Id,
            IsDeleted = examinationApprovalModel.IsDeleted,
            State = examinationApprovalModel.State,
            NewExaminationId = examinationApprovalModel.NewExaminationId,
            OldExaminationId = examinationApprovalModel.OldExaminationId
        };
        
        return examinationApproval;
    }
    public static ExaminationApprovalDomainModel ParseToModel(ExaminationApproval examinationApproval)
    {
        ExaminationApprovalDomainModel examinationApprovalModel = new ExaminationApprovalDomainModel
        {
            Id = examinationApproval.Id,
            IsDeleted = examinationApproval.IsDeleted,
            State = examinationApproval.State,
            NewExaminationId = examinationApproval.NewExaminationId,
            OldExaminationId = examinationApproval.OldExaminationId
        };
        
        return examinationApprovalModel;
    }
    public async Task<IEnumerable<ExaminationApprovalDomainModel>> ReadAll()
    {
        IEnumerable<ExaminationApprovalDomainModel> examinationApprovals = await GetAll();
        List<ExaminationApprovalDomainModel> result = new List<ExaminationApprovalDomainModel>();
        foreach (ExaminationApprovalDomainModel item in examinationApprovals)
        {
            if (!item.IsDeleted) result.Add(item);
        }
        return result;
    }

    public async Task<ExaminationApprovalDomainModel> HandleReject(decimal approvalId)
    {
        ExaminationApproval examinationApproval = await _examinationApprovalRepository.GetExaminationApprovalById(approvalId);
        examinationApproval.State = "rejected";
        _ = _examinationApprovalRepository.Update(examinationApproval);
        _examinationApprovalRepository.Save();
        return ParseToModel(examinationApproval);
    }

    public async Task<ExaminationApprovalDomainModel> Reject(decimal id)
    {
        ExaminationApproval examinationApproval = await _examinationApprovalRepository.GetExaminationApprovalById(id);
        if (!examinationApproval.State.Equals("created")) 
            throw new AlreadyHandledException();
        
        return await HandleReject(examinationApproval.Id);
    }

    public async Task<ExaminationDomainModel> ApproveDeletion(decimal id)
    {
        Examination newExamination = await _examinationRepository.GetExamination(id);
        newExamination.IsDeleted = false;
        _ = _examinationRepository.Update(newExamination);
        return ExaminationService.ParseToModel(newExamination);
    }

    public async Task<ExaminationApprovalDomainModel> HandleApproval(decimal approvalId, decimal newId, decimal oldId)
    {
        ExaminationApproval examinationApproval = await _examinationApprovalRepository.GetExaminationApprovalById(approvalId);
        examinationApproval.State = "approved";

        Examination oldExamination = await _examinationRepository.GetExamination(oldId);
        if (examinationApproval.OldExaminationId != examinationApproval.NewExaminationId)
            _ = await ApproveDeletion(newId);
        
        oldExamination.IsDeleted = true;
        
        _ = _examinationApprovalRepository.Update(examinationApproval);
        _ = _examinationRepository.Update(oldExamination);
        _examinationApprovalRepository.Save();
        _examinationRepository.Save();
        return ParseToModel(examinationApproval);
    }

    public async Task<ExaminationApprovalDomainModel> Approve(decimal id)
    {
        ExaminationApproval examinationApproval = await _examinationApprovalRepository.GetExaminationApprovalById(id);
        if (!examinationApproval.State.Equals("created"))
            throw new AlreadyHandledException();
        
        return await HandleApproval(examinationApproval.Id, examinationApproval.NewExaminationId, examinationApproval.OldExaminationId);
    }

}