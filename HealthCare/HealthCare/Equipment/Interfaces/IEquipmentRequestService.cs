using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Models;

namespace HealthCare.Domain.Interfaces;

public interface IEquipmentRequestService : IService<EquipmentRequestDomainModel>
{
    public Task<IEnumerable<EquipmentDomainModel>> GetMissingEquipment();
    public Task<IEnumerable<EquipmentRequestDomainModel>> OrderEquipment(IEnumerable<EquipmentRequestDTO> dtos);
    public Task<IEnumerable<EquipmentRequestDomainModel>> DoAllOrders();
    public Task<IEnumerable<RoomAndEquipmentDTO>> ShowRoomAndEquipment();
    public Task<EquipmentDomainModel> TransferEquipment(TransferEquipmentDTO dto);
}
