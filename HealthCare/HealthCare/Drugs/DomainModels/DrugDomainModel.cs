using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Models
{
    public class DrugDomainModel
    {
        public decimal Id { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public List<DrugIngredientDomainModel> DrugIngredients { get; set; }

    }
}
