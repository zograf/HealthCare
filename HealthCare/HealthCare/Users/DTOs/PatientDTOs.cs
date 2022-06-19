using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.DTOs
{
    public class CUPatientDTO
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public CUMedicalRecordDTO MedicalRecordDTO { get; set; }
        public LoginDTO LoginDTO { get; set; }
    }

    public class NotificationOffsetDTO
    {
        public decimal PatientId { get; set; }
        public decimal NotificationOffset { get; set; }
    }
}
