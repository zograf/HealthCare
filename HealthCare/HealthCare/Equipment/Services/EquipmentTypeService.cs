using HealthCare.Data.Entities;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Repositories;

namespace HealthCare.Domain.Services;

public class EquipmentTypeService : IEquipmentTypeService 
{
    private IEquipmentTypeRepository _equipmentTypeRepository;

    public EquipmentTypeService(IEquipmentTypeRepository equipmentTypeRepository)
    {
        _equipmentTypeRepository = equipmentTypeRepository;
    }

    public async Task<IEnumerable<EquipmentTypeDomainModel>> GetAll()
    {
        IEnumerable<EquipmentType> equipmentTypes = await _equipmentTypeRepository.GetAll();
        if (equipmentTypes == null)
            return new List<EquipmentTypeDomainModel>();

        List<EquipmentTypeDomainModel> results = new List<EquipmentTypeDomainModel>();
        EquipmentTypeDomainModel equipmentTypeModel;
        foreach (EquipmentType item in equipmentTypes)
            results.Add(ParseToModel(item));

        return results;
    }

    public static EquipmentTypeDomainModel ParseToModel(EquipmentType equipmentType)
    {
        EquipmentTypeDomainModel equipmentTypeModel = new EquipmentTypeDomainModel
        {
            Id = equipmentType.Id,
            IsDeleted = equipmentType.IsDeleted,
            Name = equipmentType.Name
        };
        
        return equipmentTypeModel;
    }
    
    public static EquipmentType ParseFromModel(EquipmentTypeDomainModel equipmentTypeModel)
    {
        EquipmentType equipmentType = new EquipmentType
        {
            Id = equipmentTypeModel.Id,
            IsDeleted = equipmentTypeModel.IsDeleted,
            Name = equipmentTypeModel.Name
        };
        
        return equipmentType;
    }

    public async Task<IEnumerable<EquipmentTypeDomainModel>> ReadAll()
    {
        IEnumerable<EquipmentTypeDomainModel> equipmentTypes = await GetAll();
        List<EquipmentTypeDomainModel> result = new List<EquipmentTypeDomainModel>();
        foreach (var item in equipmentTypes)
        {
            if (!item.IsDeleted) result.Add(item);
        }
        return result;
    }
}