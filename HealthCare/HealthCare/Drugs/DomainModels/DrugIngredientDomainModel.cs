using HealthCare.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Models
{
    public class DrugIngredientDomainModel
    {
        public decimal IngredientId { get; set; }

        public decimal DrugId { get; set; }

        public decimal Amount { get; set; }

        public bool IsDeleted { get; set; }

        public IngredientDTO? Ingredient { get; set; }
    }
}
