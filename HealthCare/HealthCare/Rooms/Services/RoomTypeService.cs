using HealthCare.Data.Entities;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Repositories;

namespace HealthCare.Domain.Services;

public class RoomTypeService : IRoomTypeService
{
    private IRoomTypeRepository _roomTypeRepository;

    public RoomTypeService(IRoomTypeRepository roomTypeRepository) 
    {
        _roomTypeRepository = roomTypeRepository;
    }

    public async Task<IEnumerable<RoomTypeDomainModel>> GetAll()
    {
        IEnumerable<RoomType> roomTypes = await _roomTypeRepository.GetAll();
        if (roomTypes == null)
            return new List<RoomTypeDomainModel>();

        List<RoomTypeDomainModel> results = new List<RoomTypeDomainModel>();
        RoomTypeDomainModel roomTypeModel;
        foreach (RoomType item in roomTypes)
        {
            results.Add(ParseToModel(item));
        }

        return results;
    } 
    public async Task<IEnumerable<RoomTypeDomainModel>> ReadAll()
    {
        IEnumerable<RoomTypeDomainModel> roomTypes = await GetAll();
        List<RoomTypeDomainModel> result = new List<RoomTypeDomainModel>();
        foreach (RoomTypeDomainModel item in roomTypes)
        {
            if (!item.IsDeleted) result.Add(item);
        }
        return result;
    }

    public static RoomTypeDomainModel ParseToModel(RoomType roomType)
    {
        RoomTypeDomainModel roomTypeModel = new RoomTypeDomainModel
        {
            IsDeleted = roomType.IsDeleted,
            Id = roomType.Id,
            RoleName = roomType.RoleName,
            Purpose = roomType.Purpose
        };
        return roomTypeModel;
    }
    
    public static RoomType ParseFromModel(RoomTypeDomainModel roomTypeModel)
    {
        RoomType roomType = new RoomType
        {
            IsDeleted = roomTypeModel.IsDeleted,
            Id = roomTypeModel.Id,
            RoleName = roomTypeModel.RoleName,
            Purpose = roomTypeModel.Purpose
        };
        
        return roomType;
    }
}