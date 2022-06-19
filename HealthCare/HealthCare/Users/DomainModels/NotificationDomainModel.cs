namespace HealthCare.Domain.Models;

public class NotificationDomainModel 
{
 
    public decimal Id { get; set; }

    public string Content { get; set; }

    public string Title { get; set; }
    
    public bool IsSeen { get; set; }
    
    public decimal CredentialsId { get; set; }
}
