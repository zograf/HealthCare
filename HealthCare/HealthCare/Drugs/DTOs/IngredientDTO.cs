using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.DTOs
{
    public class IngredientDTO
    {
        public decimal Id { get; set; }

        public string Name { get; set; }

        public bool IsAllergen { get; set; }
    }
}
