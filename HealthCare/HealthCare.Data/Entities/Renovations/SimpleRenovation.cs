using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{
    [Table("renovation")]
    public class SimpleRenovation : Renovation
    {
        [Column("room_id")]
        public decimal RoomId { get; set; }
    }
}
