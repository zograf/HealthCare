using HealthCare.Domain.DTOs;
using HealthCare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Interfaces
{
    public interface IIngredientService : IService<IngredientDomainModel>
    {
        public IngredientDomainModel Create(IngredientDTO dto);
        public IngredientDomainModel Update(IngredientDTO dto);
        public Task<IngredientDomainModel> Delete(decimal id);
        public Task<IngredientDomainModel> Get(decimal id);
        public Task<IEnumerable<IngredientDomainModel>> GetAll();
        public void Save();
    }
}
