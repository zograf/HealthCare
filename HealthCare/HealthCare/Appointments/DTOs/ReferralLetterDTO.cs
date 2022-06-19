using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.DTOs
{
    public class ReferralLetterDTO
    {
        public decimal FromDoctorId { get; set; }

        public decimal? ToDoctorId { get; set; }

        public decimal PatientId { get; set; }

        public decimal? SpecializationId { get; set; }

    }
}
