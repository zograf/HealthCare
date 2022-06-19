using HealthCare.Domain.DTOs;

namespace HealthCareAPI.Renovations.DTOs
{
    public class CreateJoinRenovationDTO : CreateRenovationDTO
    {
        public string resultRoomName { get; set; }
        public decimal roomTypeId { get; set; }
        public decimal JoinRoomId1 { get; set; }
        public decimal JoinRoomId2 { get; set; }
    }
}
