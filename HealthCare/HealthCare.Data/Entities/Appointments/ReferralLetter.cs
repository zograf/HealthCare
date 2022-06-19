using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{
    [Table("referral_letter")]
    public class ReferralLetter
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        [Column("from_doctor_id")]
        public decimal FromDoctorId { get; set; }

        [Column("to_doctor_id")]
        public decimal? ToDoctorId { get; set; }

        [Column("state")]
        public string State { get; set; }

        [Column("patient_id")]
        public decimal PatientId { get; set; }
        
        [Column("specialization_id")]
        public decimal? SpecializationId { get; set; }

        public MedicalRecord? MedicalRecord { get; set; }

        public Specialization? Specialization { get; set; }
    }
}
