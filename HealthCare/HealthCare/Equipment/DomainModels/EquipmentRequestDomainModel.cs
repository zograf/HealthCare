using HealthCare.Domain.Models;

namespace HealthCare.Data.Entities
{
    public class EquipmentRequestDomainModel
    {
        public decimal Id { get; set; }
        
        public decimal EquipmentId { get; set; }

        public bool IsExecuted  { get; set; }

        public decimal Amount { get; set; }
        
        public DateTime ExecutionTime { get; set; }
        
        public EquipmentDomainModel Equipment { get; set; }
    }

}
