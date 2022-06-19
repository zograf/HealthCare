using HealthCare.Domain.Models;

namespace HealthCare.Domain.Interfaces;

public interface IRoomTypeService : IService<RoomTypeDomainModel>
{
    public Task<IEnumerable<RoomTypeDomainModel>> ReadAll();
}