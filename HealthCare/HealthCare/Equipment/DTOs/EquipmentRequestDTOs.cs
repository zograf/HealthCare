namespace HealthCare.Domain.DTOs
{
    public class EquipmentRequestDTO
    {
        public decimal EquipmentId { get; set; }
        public decimal Amount { get; set; }
    }
    
    public class RoomAndEquipmentDTO
    {
        public string RoomName { get; set; }
        public List<EquipmentForRoomDTO> Equipment { get; set; }
    }

    public class EquipmentForRoomDTO
    {
        public string EquipmentName { get; set; }
        public decimal Amount { get; set; }
    }

    public class TransferEquipmentDTO
    {
       public decimal EquipmentId { get; set; } 
       public decimal Amount { get; set; } 
       public decimal FromRoomId { get; set; } 
       public decimal ToRoomId { get; set; } 
    }
}