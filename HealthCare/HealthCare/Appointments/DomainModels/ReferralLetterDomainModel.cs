using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Models
{
    public class ReferralLetterDomainModel
    {
        public decimal Id { get; set; }
        public decimal FromDoctorId { get; set; }
        public decimal? ToDoctorId { get; set; }
        public string State { get; set; }
        public decimal PatientId { get; set; }
        public decimal? SpecializationId { get; set; }
        public MedicalRecordDomainModel MedicalRecord { get; set; }
        public SpecializationDomainModel? Specialization { get; set; }
    }
}
