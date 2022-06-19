using HealthCare.Data.Entities;

namespace HealthCare.Domain.Models;

public class EquipmentDomainModel
{
    public decimal Id { get; set; }

    public string Name { get; set; }

    public decimal EquipmentTypeId { get; set; }

    public bool IsDynamic { get; set; }

    public bool IsDeleted { get; set; }

    public EquipmentTypeDomainModel EquipmentType { get; set; }
}