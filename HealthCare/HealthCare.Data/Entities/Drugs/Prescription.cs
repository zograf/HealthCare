using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{
    [Table("prescription")]
    public class Prescription
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public decimal Id { get; set; }

        [Column("drug_id")]
        public decimal DrugId { get; set; }

        [Column("patient_id")]
        public decimal PatientId { get; set; }

        [Column("doctor_id")]
        public decimal DoctorId { get; set; }

        [Column("therapy_time")]
        public DateTime TakeAt { get; set; }

        [Column("per_day")]
        public decimal PerDay { get; set; }

        [Column("meal_combination")]
        public string MealCombination { get; set; }

        [Column("treatment_days")]
        public decimal TreatmentDays { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        public Drug Drug { get; set; }
        public MedicalRecord MedicalRecord { get; set; }

    }
}
