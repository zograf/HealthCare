using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.DTOs
{
    public class DrugDTO
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public Dictionary<decimal, decimal> IngredientAmounts { get; set; }
    }
}
