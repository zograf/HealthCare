using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.DTOs
{
    public class DrugIngredientDTO
    {
        public decimal IngredientId { get; set; }

        public decimal DrugId { get; set; }

        public decimal Amount { get; set; }

    }
}
