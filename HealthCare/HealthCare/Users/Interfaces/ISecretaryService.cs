using HealthCare.Domain.Models;

namespace HealthCare.Domain.Interfaces;

public interface ISecretaryService : IService<SecretaryDomainModel>
{
    public Task<IEnumerable<SecretaryDomainModel>> ReadAll();
    public Task<SecretaryDomainModel> GetById(decimal id);
}