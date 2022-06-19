using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{
    [Table("join_renovation")]
    public class JoinRenovation : Renovation
    {
        [Column("result_room_id")]
        public decimal ResultRoomId { get; set; }

        [Column("join_room1_id")]
        public decimal JoinRoomId1 { get; set; }

        [Column("join_room2_id")]
        public decimal JoinRoomId2 { get; set; }
    }
}
