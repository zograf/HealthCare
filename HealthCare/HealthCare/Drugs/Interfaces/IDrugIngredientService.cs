using HealthCare.Domain.DTOs;
using HealthCare.Domain.Models;

namespace HealthCare.Domain.Interfaces;

public interface IDrugIngredientService : IService<DrugIngredientDomainModel>
{
    public DrugIngredientDomainModel Create(DrugIngredientDTO dto);
    Task<DrugIngredientDomainModel> Delete(decimal drugId, decimal ingredientId);
    DrugIngredientDomainModel Update(DrugIngredientDTO drugIngredientDTO);
}