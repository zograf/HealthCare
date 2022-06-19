using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities 
{

    [Table("anti_troll")]
    public class AntiTroll 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public decimal Id { get; set; }

        [Column("state")]
        public string State { get; set; }

        [Column("date")]
        public DateTime DateCreated { get; set; }

        [Column("patient_id")]
        public decimal PatientId { get; set; }
    }
}
