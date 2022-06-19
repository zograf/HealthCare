using HealthCare.Domain.DTOs;

namespace HealthCareAPI.Renovations.DTOs
{
    public class CreateSimpleRenovationDTO : CreateRenovationDTO
    {
        public decimal RoomId { get; set; }
    }
}
