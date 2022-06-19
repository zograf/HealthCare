using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{
    [Table("inventory")]
    public class Inventory
    {
        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("room_id")]
        public decimal RoomId { get; set; }

        [Column("equipment_id")]
        public decimal EquipmentId { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        public Equipment Equipment { get; set; }
    }
}
