using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Models
{
    public class AllergyDomainModel
    {
        public decimal PatientId  { get; set; }

        public IngredientDomainModel? Ingredient { get; set; }

        public bool IsDeleted { get; set; }

    }
}
