using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Models;

namespace HealthCare.Domain.Interfaces;

public interface IAnamnesisService : IService<AnamnesisDomainModel> 
{
    public Task<AnamnesisDomainModel> Create(CreateAnamnesisDTO dto);
    public Task<IEnumerable<AnamnesisDomainModel>> ReadAll();
}