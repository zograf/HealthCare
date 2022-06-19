using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{
    [Table("equipment_type")]
    public class EquipmentType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public decimal Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }
    }

}
