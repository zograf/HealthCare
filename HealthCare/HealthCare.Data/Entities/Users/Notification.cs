using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare.Data.Entities;

[Table("notification")]
public class Notification
{
    [Column("id")] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public decimal Id { get; set; }
    
    [Column("description")] 
    public string Content { get; set; }
    
    [Column("title")] 
    public string Title { get; set; }
    
    [Column("cred_id")] 
    public decimal CredentialsId { get; set; }
    
    [Column("seen")] 
    public bool IsSeen { get; set; }
}