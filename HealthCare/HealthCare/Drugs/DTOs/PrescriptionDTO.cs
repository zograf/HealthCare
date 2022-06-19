using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.DTOs
{
    public class PrescriptionDTO
    {
        public decimal DrugId { get; set; }

        public decimal PatientId { get; set; }

        public decimal DoctorId { get; set; }

        public DateTime TakeAt { get; set; }

        public decimal PerDay { get; set; }

        public decimal TreatmentDays { get; set; }

        public string MealCombination { get; set; }
    }
}
