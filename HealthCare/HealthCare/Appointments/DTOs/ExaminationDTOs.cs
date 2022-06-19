using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.DTOs
{
    public class SortExaminationDTO
    {
        public decimal PatientId { get; set; }
        public string SortParam { get; set; }
    }

    public class DeleteExaminationDTO
    {
        public decimal ExaminationId { get; set; }
        public decimal PatientId { get; set; }
        public bool IsPatient { get; set; }
    }

    public class CUExaminationDTO
    {
        public decimal DoctorId { get; set; }
        public decimal ExaminationId { get; set; }
        public decimal PatientId { get; set; }
        public DateTime StartTime { get; set; }
        public bool IsPatient { get; set; }
    }

    public class CreateUrgentExaminationDTO
    {
        public decimal PatientId { get; set; }
        public decimal SpecializationId { get; set; }
    }

    public class SearchByNameDTO
    {
        public decimal PatientId { get; set; }
        public string Substring { get; set; }
    }

    public class ParamsForRecommendingFreeExaminationsDTO
    {
        public decimal PatientId { get; set; }
        
        public decimal DoctorId { get; set; }
        
        public DateTime TimeFrom { get; set; }
        
        public DateTime TimeTo { get; set; }
        
        public DateTime LastDate { get; set; }
        
        public bool IsDoctorPriority { get; set; }
    }

    public class RescheduleDTO
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime UrgentStartTime { get; set; }
        public decimal DoctorId { get; set; }
        public decimal PatientId { get; set; }
        public DateTime RescheduleTime { get; set; }
        public decimal Duration { get; set; }
    }
}
