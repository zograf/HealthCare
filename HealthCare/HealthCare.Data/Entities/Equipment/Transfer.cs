using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{
    [Table("transfer")]
    public class Transfer
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        [Column("room_id_out")]
        public decimal RoomIdOut { get; set; }

        [Column("equipment_id")]
        public decimal EquipmentId { get; set; }

        [Column("room_id_in")]
        public decimal RoomIdIn { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("time_transfered")]
        public DateTime TransferTime { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        [Column("executed")]
        public bool Executed { get; set; }

        public Equipment Equipment { get; set; }
    }
}
