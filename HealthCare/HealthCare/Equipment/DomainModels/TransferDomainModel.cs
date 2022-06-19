using HealthCare.Data.Entities;

namespace HealthCare.Domain.Models;

public class TransferDomainModel
{
    public decimal RoomIdOut { get; set; }

    public decimal EquipmentId { get; set; }

    public decimal RoomIdIn { get; set; }

    public decimal Amount { get; set; }

    public DateTime TransferTime { get; set; }

    public bool IsDeleted { get; set; }

    public EquipmentDomainModel Equipment { get; set; }

    public decimal Id { get; set; }

    public bool Executed { get; set; }

}