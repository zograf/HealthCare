using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.DTOs
{
    public class CUOperationDTO
    {
        public decimal Id { get; set; }

        public DateTime StartTime { get; set; }

        public decimal Duration { get; set; }

        public decimal DoctorId { get; set; }

        public decimal PatientId { get; set; }
    }
    
    public class CreateUrgentOperationDTO
    {
        public decimal PatientId { get; set; }
        public decimal SpecializationId { get; set; }
        public decimal Duration { get; set; }
    }
}
