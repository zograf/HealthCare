using HealthCare.Domain.Models;

namespace HealthCare.Domain.Interfaces;

public interface IEquipmentTypeService : IService<EquipmentTypeDomainModel>
{
    public Task<IEnumerable<EquipmentTypeDomainModel>> ReadAll();
}