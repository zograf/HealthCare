using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Repositories;

namespace HealthCare.Domain.Services;

public class DrugIngredientService : IDrugIngredientService
{
    private readonly IDrugIngredientRepository _drugIngredientRepository;
    public DrugIngredientService(IDrugIngredientRepository drugIngredientRepository)
    {
        _drugIngredientRepository = drugIngredientRepository;
    }

    public static DrugIngredientDomainModel ParseToModel(DrugIngredient drugIngredient)
    {
        DrugIngredientDomainModel drugIngredientModel = new DrugIngredientDomainModel
        {
            DrugId = drugIngredient.DrugId,
            Amount = drugIngredient.Amount,
            IngredientId = drugIngredient.IngredientId,
            IsDeleted = drugIngredient.IsDeleted
        };

        if (drugIngredient.Ingredient != null)
            drugIngredientModel.Ingredient = IngredientService.ParseToDTO(drugIngredient.Ingredient);

        
        return drugIngredientModel;
    }

    public static DrugIngredient ParseFromModel(DrugIngredientDomainModel drugIngredientModel)
    {
        DrugIngredient drugIngredient = new DrugIngredient
        {
            DrugId = drugIngredientModel.DrugId,
            Amount = drugIngredientModel.Amount,
            IngredientId = drugIngredientModel.IngredientId,
            IsDeleted = drugIngredientModel.IsDeleted
        };
        
        return drugIngredient;
    }

    public async Task<IEnumerable<DrugIngredientDomainModel>> GetAll()
    {
        IEnumerable<DrugIngredient> drugIngredients = await _drugIngredientRepository.GetAll();
        return ParseToModel(drugIngredients);
    }

    private IEnumerable<DrugIngredientDomainModel> ParseToModel(IEnumerable<DrugIngredient> drugIngredients)
    {
        List<DrugIngredientDomainModel> drugIngredientModels = new List<DrugIngredientDomainModel>();
        foreach(DrugIngredient drugIngredient in drugIngredients)
        {
            drugIngredientModels.Add(ParseToModel(drugIngredient));
        }
        return drugIngredientModels;
    }

    public DrugIngredientDomainModel Create(DrugIngredientDTO dto)
    {
        DrugIngredient drugIngredient = new DrugIngredient
        {
            DrugId = dto.DrugId,
            IngredientId = dto.IngredientId,
            Amount = dto.Amount,
        };
        _drugIngredientRepository.Post(drugIngredient);
        _drugIngredientRepository.Save();
        return ParseToModel(drugIngredient);
    }

    public async Task<DrugIngredientDomainModel> Delete(decimal drugId, decimal ingredientId)
    {
        DrugIngredient drugIngredient = await _drugIngredientRepository.GetById(drugId, ingredientId);
        drugIngredient.IsDeleted = true;
        _drugIngredientRepository.Update(drugIngredient);
        _drugIngredientRepository.Save();
        return ParseToModel(drugIngredient);
    }

    public DrugIngredientDomainModel Update(DrugIngredientDTO drugIngredientDTO)
    {
        DrugIngredient drugIngredient = new DrugIngredient
        {
            DrugId = drugIngredientDTO.DrugId,
            IngredientId = drugIngredientDTO.IngredientId,
            Amount = drugIngredientDTO.Amount,
        };
        _drugIngredientRepository.Update(drugIngredient);
        _drugIngredientRepository.Save();
        return ParseToModel(drugIngredient);
    }
}