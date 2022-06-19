using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Domain.Models;

namespace HealthCare.Domain.DTOs
{
    public class DoctorsScheduleDTO
    {
        public decimal DoctorId { get; set; }
        public DateTime Date { get; set; }
        public bool ThreeDays { get; set; }
    }

    public class CreateUrgentAppointmentDTO
    {
        public bool IsExamination { get; set; }
        public decimal Duration { get; set; }
        public decimal PatientId { get; set; }
        public decimal SpecializationId { get; set; }
    }
}
