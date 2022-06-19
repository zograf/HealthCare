using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare.Data.Entities
{
    [Table("equipment_request")]
    public class EquipmentRequest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public decimal Id { get; set; }
        
        [Column("equipment_id")]
        public decimal EquipmentId { get; set; }

        [Column("executed")]
        public bool IsExecuted  { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }
        
        [Column("execution_time")]
        public DateTime ExecutionTime { get; set; }
        
        public Equipment Equipment { get; set; }
    }

}
