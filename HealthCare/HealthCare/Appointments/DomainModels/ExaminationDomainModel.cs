using HealthCare.Data.Entities;

namespace HealthCare.Domain.Models;

public class ExaminationDomainModel : AppointmentDomainModel
{
    public AnamnesisDomainModel? Anamnesis;

}