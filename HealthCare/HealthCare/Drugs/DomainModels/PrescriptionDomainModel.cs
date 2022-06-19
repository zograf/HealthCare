using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Models
{
    public enum MealCombination
    {
        AfterMeal,
        BeforeMeal,
        NotImportant
    }

    public class PrescriptionDomainModel
    {
        public decimal Id { get; set; }

        public decimal DrugId { get; set; }

        public decimal PatientId { get; set; }

        public decimal DoctorId { get; set; }

        public DateTime TakeAt { get; set; }

        public decimal PerDay { get; set; }

        public decimal TreatmentDays { get; set; }

        public bool IsDeleted { get; set; }

        public MealCombination MealCombination { get; set; }

        public DrugDomainModel Drug { get; set; }

        public MedicalRecordDomainModel MedicalRecord { get; set; }
    }
}
