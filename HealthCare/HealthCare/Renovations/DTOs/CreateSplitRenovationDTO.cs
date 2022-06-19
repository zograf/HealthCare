using HealthCare.Domain.DTOs;

namespace HealthCareAPI.Renovations.DTOs
{
    public class CreateSplitRenovationDTO : CreateRenovationDTO
    {
        public string resultRoomName1 { get; set; }
        public decimal roomTypeId1 { get; set; }
        public string resultRoomName2 { get; set; }
        public decimal roomTypeId2 { get; set; }

        public decimal SplitRoomId { get; set; }

    }
}
