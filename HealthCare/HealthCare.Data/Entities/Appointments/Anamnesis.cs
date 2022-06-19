using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{
    [Table("anamnesis")]
    public class Anamnesis
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public decimal Id { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("examination_id")]
        public decimal ExaminationId { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        public Examination Examination { get; set; }
    }
}
