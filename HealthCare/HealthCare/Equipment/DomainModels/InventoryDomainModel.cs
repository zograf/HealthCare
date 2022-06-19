using HealthCare.Data.Entities;

namespace HealthCare.Domain.Models;

public class InventoryDomainModel
{
    public decimal Amount { get; set; }

    public decimal RoomId { get; set; }

    public decimal EquipmentId { get; set; }

    public bool IsDeleted { get; set; }

    public EquipmentDomainModel Equipment { get; set; }
}