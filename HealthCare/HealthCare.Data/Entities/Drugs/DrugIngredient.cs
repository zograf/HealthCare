using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{
    [Table("drug_ingredient")]
    public class DrugIngredient
    {
        [Column("ingredient_id")]
        public decimal IngredientId { get; set; }

        [Column("drug_id")]
        public decimal DrugId { get; set; }

        [Column("ammount")]
        public decimal Amount { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        public Ingredient Ingredient { get; set; }
        
        public Drug Drug { get; set; }
    }
}
