using HealthCare.Data.Entities;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Repositories;

namespace HealthCare.Domain.Services;

public class SecretaryService : ISecretaryService
{
    private ISecretaryRepository _secretaryRepository;

    public SecretaryService(ISecretaryRepository secretaryRepository) 
    {
        _secretaryRepository = secretaryRepository;
    }
    
    public async Task<IEnumerable<SecretaryDomainModel>> ReadAll()
    {
        IEnumerable<SecretaryDomainModel> secretaries = await GetAll();
        List<SecretaryDomainModel> result = new List<SecretaryDomainModel>();
        foreach (SecretaryDomainModel item in secretaries)
        {
            if (!item.IsDeleted) result.Add(item);
        }
        return result;
    } 

    // Async awaits info from database
    // GetAll is the equivalent of SELECT *
    public async Task<IEnumerable<SecretaryDomainModel>> GetAll()
    {
        IEnumerable<Secretary> data = await _secretaryRepository.GetAll();
        if (data == null)
            return new List<SecretaryDomainModel>();

        List<SecretaryDomainModel> results = new List<SecretaryDomainModel>();
        foreach (Secretary secretary in data)
        {
            results.Add(ParseToModel(secretary));
        }

        return results;
    }

    public async Task<SecretaryDomainModel> GetById(decimal id)
    {
        Secretary secretary = await _secretaryRepository.GetById(id);
        if (secretary == null)
            throw new DataIsNullException();

        return ParseToModel(secretary);
    }

    public static SecretaryDomainModel ParseToModel(Secretary secretary)
    {
        SecretaryDomainModel secretaryModel = new SecretaryDomainModel
        {
            IsDeleted = secretary.IsDeleted,
            Id = secretary.Id,
            BirthDate = secretary.BirthDate,
            Email = secretary.Email,
            Name = secretary.Name,
            Phone = secretary.Phone,
            Surname = secretary.Surname
        };
        if (secretary.Credentials != null)
            secretaryModel.Credentials = CredentialsService.ParseToModel(secretary.Credentials);

        return secretaryModel;
    }
    
    public static Secretary ParseFromModel(SecretaryDomainModel secretaryModel)
    {
        Secretary secretary = new Secretary
        {
            IsDeleted = secretaryModel.IsDeleted,
            Id = secretaryModel.Id,
            BirthDate = secretaryModel.BirthDate,
            Email = secretaryModel.Email,
            Name = secretaryModel.Name,
            Phone = secretaryModel.Phone,
            Surname = secretaryModel.Surname
        };
        
        if (secretaryModel.Credentials != null)
            secretary.Credentials = CredentialsService.ParseFromModel(secretaryModel.Credentials);

        return secretary;
    }
}