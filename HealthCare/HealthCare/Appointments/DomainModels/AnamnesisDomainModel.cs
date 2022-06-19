using System.ComponentModel.DataAnnotations.Schema;
using HealthCare.Data.Entities;

namespace HealthCare.Domain.Models;

public class AnamnesisDomainModel 
{
 
    public decimal Id { get; set; }

    
    public string Description { get; set; }

    
    public decimal ExaminationId { get; set; }
    
    public bool IsDeleted { get; set; }
}