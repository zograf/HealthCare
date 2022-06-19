using HealthCare.Data.Entities;

namespace HealthCare.Domain.Models;

public class RoomTypeDomainModel
{
    public decimal Id { get; set; }

    public string RoleName { get; set; }

    public bool IsDeleted { get; set; }

    public string Purpose { get; set; }

}