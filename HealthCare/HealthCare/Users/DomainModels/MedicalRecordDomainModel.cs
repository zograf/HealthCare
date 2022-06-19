using HealthCare.Data.Entities;

namespace HealthCare.Domain.Models;

public class MedicalRecordDomainModel
{
    public decimal Height { get; set; }

    public decimal Weight { get; set; }

    public string BedriddenDiseases { get; set; }

    public decimal PatientId { get; set; }

    public bool IsDeleted { get; set; }

    public List<ReferralLetterDomainModel> ReferralLetters { get; set; }

    public List<PrescriptionDomainModel> Prescriptions {get; set; }

    public List<AllergyDomainModel> AllergiesList { get; set; }


}
