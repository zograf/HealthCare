using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Interfaces
{
    public interface IDrugService : IService<DrugDomainModel>
    {
        public Task<DrugDomainModel> Create(DrugDTO dto);
        public Task<DrugDomainModel> Update(DrugDTO dto);
        public Task AddIngredients(DrugDTO dto, Drug drug);
    }
}
