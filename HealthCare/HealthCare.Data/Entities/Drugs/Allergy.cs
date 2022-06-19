using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{
    [Table("allergy")]
    public class Allergy
    {
        [Column("medical_record_id")]
        public decimal PatientId { get; set; }

        [Column("ingredient_id")]
        public decimal IngredientId { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        public Ingredient? Ingredient { get; set; }

        public MedicalRecord? MedicalRecord { get; set; }   
    }
}
