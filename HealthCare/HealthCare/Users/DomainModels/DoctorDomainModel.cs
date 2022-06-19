using HealthCare.Data.Entities;

namespace HealthCare.Domain.Models;

public class DoctorDomainModel
{
    public decimal Id { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string Email { get; set; }

    public DateTime BirthDate { get; set; }

    public string Phone { get; set; }

    public bool IsDeleted { get; set; }

    public decimal Rating { get; set; }
    public decimal SpecializationId { get; set; }

    public List<OperationDomainModel> Operations { get; set; }

    public List<ExaminationDomainModel> Examinations { get; set; }

    public CredentialsDomainModel Credentials { get; set; }

    public SpecializationDomainModel? Specialization { get; set; }

}