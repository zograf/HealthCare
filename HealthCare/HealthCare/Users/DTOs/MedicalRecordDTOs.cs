using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.DTOs
{
    public class CUMedicalRecordDTO
    {
        public decimal Height { get; set; }

        public decimal Weight { get; set; }

        public string BedriddenDiseases { get; set; }

        public decimal PatientId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
