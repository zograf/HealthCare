using HealthCare.Data.Entities;

namespace HealthCare.Domain.Models;

public class PatientDomainModel
{
    public decimal Id { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string Email { get; set; }

    public DateTime BirthDate { get; set; }

    public string Phone { get; set; }

    public string BlockedBy { get; set; }

    public decimal BlockingCounter { get; set; }

    public decimal NotificationOffset { get; set; }

    public bool IsDeleted { get; set; }

    public List<OperationDomainModel> Operations { get; set; }

    public List<ExaminationDomainModel> Examinations { get; set; }

    public MedicalRecordDomainModel MedicalRecord { get; set; }
    
    public CredentialsDomainModel Credentials { get; set; }

}