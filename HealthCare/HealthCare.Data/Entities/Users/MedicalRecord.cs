using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{
    [Table("medical_record")]
    public class MedicalRecord
    {
        [Column("height")]
        public decimal Height { get; set; }

        [Column("weight")]
        public decimal Weight { get; set; }

        [Column("bedridden_diseases")]
        public string BedriddenDiseases { get; set; }

        [Column("Patient_id")]
        public decimal PatientId { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        public List<ReferralLetter> ReferralLetters { get; set; }

        public List<Prescription> Prescriptions { get; set; }

        public List<Allergy> AllergiesList { get; set; }
    }
}
