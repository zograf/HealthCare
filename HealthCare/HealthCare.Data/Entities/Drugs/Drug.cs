using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{
    [Table("drug")]
    public class Drug
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        public List<DrugIngredient> DrugIngredients { get; set; }
    }
}
