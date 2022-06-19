using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{
    [Table("split_renovation")]
    public class SplitRenovation : Renovation
    {
        [Column("split_room_id")]
        public decimal SplitRoomId { get; set; }

        [Column("factor_room1_id")]
        public decimal ResultRoomId1 { get; set; }

        [Column("factor_room2_id")]
        public decimal ResultRoomId2 { get; set; }
    }
}
