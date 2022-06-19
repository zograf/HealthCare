namespace HealthCare.Domain.DTOs;

public class SendNotificationDTO
{
    public KeyValuePair<string, string> Content  { get; set; }
    public decimal PersonId { get; set; }
    public bool IsPatient { get; set; }
}