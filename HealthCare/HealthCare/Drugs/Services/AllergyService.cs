using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Repositories;

namespace HealthCare.Domain.Services;

public class AllergyService : IAllergyService
{
    private IAllergyRepository _allergyRepository;
    public AllergyService(IAllergyRepository allergyRepository)
    {
        _allergyRepository = allergyRepository;
    }

    public static AllergyDomainModel ParseToModel(Allergy allergy)
    {
        AllergyDomainModel allergyModel = new AllergyDomainModel
        {
            PatientId = allergy.PatientId,
            IsDeleted = allergy.IsDeleted
        };

        if (allergy.Ingredient != null)
            allergyModel.Ingredient = IngredientService.ParseToModel(allergy.Ingredient);
        
        return allergyModel;
    }
    
    public static Allergy ParseFromModel(AllergyDomainModel allergyModel)
    {
        Allergy allergy = new Allergy
        {
            IngredientId = allergyModel.Ingredient.Id,
            PatientId = allergyModel.PatientId,
            IsDeleted = allergyModel.IsDeleted
        };
        
        return allergy;
    }

    public async Task<IEnumerable<AllergyDomainModel>> GetAll()
    {
        IEnumerable<Allergy> data = await _allergyRepository.GetAll();
        if (data == null)
            return new List<AllergyDomainModel>();

        List<AllergyDomainModel> results = new List<AllergyDomainModel>();
        foreach (Allergy item in data)
        {
            results.Add(ParseToModel(item));
        }

        return results;
    }

    public async Task<IEnumerable<AllergyDomainModel>> GetAllForPatient(decimal patientId)
    {
        IEnumerable<Allergy> data = await _allergyRepository.GetAllByPatientId(patientId);
        if (data == null)
            return new List<AllergyDomainModel>();

        List<AllergyDomainModel> results = new List<AllergyDomainModel>();
        foreach (Allergy item in data)
        {
            results.Add(ParseToModel(item));
        }

        return results;
    }

    public async Task<AllergyDomainModel> Create(AllergyDTO dto)
    {
        Allergy newAllergy = new Allergy
        {
            PatientId = dto.PatientId,
            IngredientId = dto.IngredientId,
            IsDeleted = false
        };

        _ = _allergyRepository.Post(newAllergy);
        _allergyRepository.Save();

        AllergyDomainModel allergyModel = ParseToModel(await _allergyRepository.GetById(dto.PatientId, dto.IngredientId));

        return allergyModel;
    }

    public async Task<AllergyDomainModel> Delete(AllergyDTO dto)
    {
        Allergy allergy = await _allergyRepository.GetById(dto.PatientId, dto.IngredientId);

        // logical delete
        allergy.IsDeleted = true;
        _ = _allergyRepository.Update(allergy);
        _allergyRepository.Save();

        return ParseToModel(allergy);
    }

}