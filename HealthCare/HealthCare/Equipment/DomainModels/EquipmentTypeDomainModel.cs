using HealthCare.Data.Entities;

namespace HealthCare.Domain.Models;

public class EquipmentTypeDomainModel
{
    public decimal Id { get; set; }

    public string Name { get; set; }

    public bool IsDeleted { get; set; }
}