using System.Data;
using HealthCare.Data.Entities;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Repositories;

namespace HealthCare.Domain.Services;

public class AntiTrollService : IAntiTrollService 
{
    private IAntiTrollRepository _antiTrollRepository;

    public AntiTrollService(IAntiTrollRepository antiTrollRepository) 
    {
        _antiTrollRepository = antiTrollRepository;
    }

    // Async awaits info from database
    // GetAll is the equivalent of SELECT *

    public static AntiTrollDomainModel ParseToModel(AntiTroll antiTroll) 
    {
        AntiTrollDomainModel antiTrollModel = new AntiTrollDomainModel 
        {
            Id = antiTroll.Id,
            PatientId = antiTroll.PatientId,
            DateCreated = antiTroll.DateCreated,
            State = antiTroll.State
        };
        return antiTrollModel;
    }
    
    public static AntiTroll ParseFromModel(AntiTrollDomainModel antiTrollModel) 
    {
        AntiTroll antiTroll = new AntiTroll 
        {
            Id = antiTrollModel.Id,
            PatientId = antiTrollModel.PatientId,
            DateCreated = antiTrollModel.DateCreated,
            State = antiTrollModel.State
        };
        return antiTroll;
    }

    public async Task<IEnumerable<AntiTrollDomainModel>> GetAll() 
    {
        IEnumerable<AntiTroll> data = await _antiTrollRepository.GetAll();
        if (data == null)
            return new List<AntiTrollDomainModel>();

        List<AntiTrollDomainModel> results = new List<AntiTrollDomainModel>();
        foreach (AntiTroll item in data) 
        {
            results.Add(ParseToModel(item));
        }
        return results;
    }

    public async Task<IEnumerable<AntiTrollDomainModel>> GetByPatientId(decimal patientId) 
    {
        IEnumerable<AntiTroll> data = await _antiTrollRepository.GetByPatientId(patientId);
        if (data == null)
            throw new DataIsNullException();

        List<AntiTrollDomainModel> results = new List<AntiTrollDomainModel>();
        foreach (AntiTroll item in data) 
        {
            results.Add(ParseToModel(item));
        }

        return results;
    }

    public async Task<bool> AntiTrollCheck(decimal patientId, bool isCreate)
    {
        IEnumerable<AntiTroll> antiTrollHistory = await _antiTrollRepository.GetByPatientId(patientId);
        int createCounter = 0;
        int updateCounter = 0;
        foreach (AntiTroll item in antiTrollHistory)
        {
            double difference = (DateTime.Now - item.DateCreated).TotalDays;
            if (difference < 30)
            {
                if (item.State.Equals("create"))
                    createCounter++;
                else
                    updateCounter++;
            }
        }
        return isCreate ? createCounter > 8 : updateCounter > 5;
    }
    public void WriteToAntiTroll(decimal patientId, string state)
    {
        AntiTroll antiTrollItem = new AntiTroll
        {
            PatientId = patientId,
            State = state,
            DateCreated = UtilityService.RemoveSeconds(DateTime.Now)
        };

        _ = _antiTrollRepository.Post(antiTrollItem);
        _antiTrollRepository.Save();
    }
}