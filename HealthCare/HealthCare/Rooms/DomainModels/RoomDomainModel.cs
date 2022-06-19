using HealthCare.Data.Entities;

namespace HealthCare.Domain.Models;

public class RoomDomainModel
{
    public decimal Id { get; set; }

    public string RoomName { get; set; }

    public decimal RoomTypeId  { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsFormed { get; set; }

    public List<InventoryDomainModel> Inventories { get; set; }
    public RoomTypeDomainModel RoomType { get; set; }
    public List<OperationDomainModel> Operations { get; set; }
}