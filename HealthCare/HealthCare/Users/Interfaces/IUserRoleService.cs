using HealthCare.Domain.Models;

namespace HealthCare.Domain.Interfaces;

public interface IUserRoleService : IService<UserRoleDomainModel>
{
    public Task<IEnumerable<UserRoleDomainModel>> ReadAll();
}