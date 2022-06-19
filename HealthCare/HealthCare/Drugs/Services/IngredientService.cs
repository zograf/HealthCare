using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Services
{
    public class IngredientService : IIngredientService
    {
        private IIngredientRepository _ingredientRepository;

        public IngredientService(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        public static IngredientDomainModel ParseToModel(Ingredient ingredient)
        {
            IngredientDomainModel ingredientModel = new IngredientDomainModel
            {
                Id = ingredient.Id,
                IsAllergen = ingredient.IsAllergen,
                Name = ingredient.Name
            };

            ingredientModel.DrugIngredients = new List<DrugIngredientDomainModel>();
            if (ingredient.DrugIngredients != null)
                foreach (DrugIngredient drugIngredient in ingredient.DrugIngredients)
                    ingredientModel.DrugIngredients.Add(DrugIngredientService.ParseToModel(drugIngredient));

            return ingredientModel;
        }

        public static IngredientDTO ParseToDTO(Ingredient ingredient)
        {
            IngredientDTO ingredientDTO = new IngredientDTO
            {
                Id = ingredient.Id,
                IsAllergen = ingredient.IsAllergen,
                Name = ingredient.Name
            };

            return ingredientDTO;
        }

        public static Ingredient ParseFromModel(IngredientDomainModel ingredientModel)
        {
            Ingredient ingredient = new Ingredient
            {
                Id = ingredientModel.Id,
                IsAllergen = ingredientModel.IsAllergen,
                Name = ingredientModel.Name
            };
            
            ingredient.DrugIngredients = new List<DrugIngredient>();
            
            if (ingredientModel.DrugIngredients != null)
                foreach (DrugIngredientDomainModel drugIngredientModel in ingredientModel.DrugIngredients)
                    ingredient.DrugIngredients.Add(DrugIngredientService.ParseFromModel(drugIngredientModel));
            
            return ingredient;
        }

        public async Task<IEnumerable<IngredientDomainModel>> GetAll()
        {
            IEnumerable<Ingredient> data = await _ingredientRepository.GetAll();
            if (data == null)
                return new List<IngredientDomainModel>();

            List<IngredientDomainModel> results = new List<IngredientDomainModel>();
            foreach (Ingredient item in data)
            {
                results.Add(ParseToModel(item));
            }

            return results;
        }

        public IngredientDomainModel Create(IngredientDTO dto)
        {
            Ingredient ingredient = new Ingredient
            {
                Id = dto.Id,
                IsAllergen = dto.IsAllergen,
                Name = dto.Name,
                IsDeleted = false,
            };
            _ingredientRepository.Post(ingredient);
            _ingredientRepository.Save();
            return parseToModel(ingredient);
        }

        private IngredientDomainModel parseToModel(Ingredient ingredient)
        {
            return new IngredientDomainModel
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                IsAllergen = ingredient.IsAllergen,
                IsDeleted = ingredient.IsDeleted,
            };
        }

        public IngredientDomainModel Update(IngredientDTO dto)
        {
            Ingredient ingredient = new Ingredient
            {
                Id = dto.Id,
                Name = dto.Name,
                IsAllergen = dto.IsAllergen,
            };
            _ingredientRepository.Update(ingredient);
            _ingredientRepository.Save();
            return parseToModel(ingredient);
        }

        public async Task<IngredientDomainModel> Delete(decimal id)
        {
            Ingredient ingredient = await _ingredientRepository.Get(id);
            ingredient.IsDeleted = true;
            _ingredientRepository.Update(ingredient);
            _ingredientRepository.Save();
            return parseToModel(ingredient);
        }

        public async Task<IngredientDomainModel> Get(decimal id)
        {
            Ingredient ingredient = await _ingredientRepository.Get(id);
            return parseToModel(ingredient);
        }

        public void Save()
        {
            _ingredientRepository.Save();
        }
    }
}
