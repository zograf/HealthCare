using System.Data;
using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Repositories;

namespace HealthCare.Domain.Services;

public class AnamnesisService : IAnamnesisService
{
    private IAnamnesisRepository _anamnesisRepository;
    private IExaminationRepository _examinationRepository;

    public AnamnesisService(IAnamnesisRepository anamnesisRepository, IExaminationRepository examinationRepository)
    {
        _anamnesisRepository = anamnesisRepository;
        _examinationRepository = examinationRepository;
    }

    public async Task<IEnumerable<AnamnesisDomainModel>> GetAll()
    {
        IEnumerable<Anamnesis> data = await _anamnesisRepository.GetAll();
        if (data == null)
            return new List<AnamnesisDomainModel>();

        List<AnamnesisDomainModel> results = new List<AnamnesisDomainModel>();
        AnamnesisDomainModel anamnesisModel;
        foreach (Anamnesis item in data)
        {
            anamnesisModel = new AnamnesisDomainModel
            {
                Id = item.Id,
                Description = item.Description,
                ExaminationId = item.ExaminationId,
                IsDeleted = item.IsDeleted
            };
            results.Add(anamnesisModel);
        }

        return results;
    }

    public async Task<IEnumerable<AnamnesisDomainModel>> ReadAll()
    {
        IEnumerable<AnamnesisDomainModel> anamnesis = await GetAll();
        List<AnamnesisDomainModel> result = new List<AnamnesisDomainModel>();
        foreach (AnamnesisDomainModel item in anamnesis)
        {
            if (!item.IsDeleted) result.Add(item);
        }

        return result;
    }

    public async Task<AnamnesisDomainModel> Create(CreateAnamnesisDTO dto)
    {
        Examination examination = await _examinationRepository.GetExamination(dto.ExaminationId);
        // can't create another anamnesis for examination that already has one
        if (examination.Anamnesis != null)
            throw new AnamnesisAlreadyExistsException();
        // can't create anamnesis for examinations that are more than 1 hour earlier than current time
        if (examination.StartTime.AddHours(-1) < DateTime.UtcNow)
            throw new DateInPastExeption();
  
        Anamnesis anamesis = new Anamnesis
        {
            ExaminationId = dto.ExaminationId,
            IsDeleted = false,
            Description = dto.Description
        };
        _ = _anamnesisRepository.Post(anamesis);
        _anamnesisRepository.Save();

        return ParseToModel(anamesis);
    }

    public static AnamnesisDomainModel ParseToModel(Anamnesis anamnesis)
    {
        AnamnesisDomainModel anamnesisModel = new AnamnesisDomainModel
        {
            Id = anamnesis.Id,
            Description = anamnesis.Description,
            ExaminationId = anamnesis.ExaminationId,
            IsDeleted = anamnesis.IsDeleted,
        };

        return anamnesisModel;
    }
    
    public static Anamnesis ParseFromModel(AnamnesisDomainModel anamnesisModel)
    {
        Anamnesis anamnesis = new Anamnesis
        {
            Id = anamnesisModel.Id,
            Description = anamnesisModel.Description,
            ExaminationId = anamnesisModel.ExaminationId,
            IsDeleted = anamnesisModel.IsDeleted,
        };

        return anamnesis;
    }
}