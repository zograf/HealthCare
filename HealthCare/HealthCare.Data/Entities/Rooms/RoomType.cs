using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{
    [Table("room_type")]
    public class RoomType
    {
        [Column("id")]
        public decimal Id { get; set; }

        [Column("name")]
        public string RoleName { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        [Column("purpose")]
        public string Purpose { get; set; }
    }
}   
