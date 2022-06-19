using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{

    [Table("examination_approval")]
    public class ExaminationApproval
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public decimal Id { get; set; }
        [Column("state")]
        public string State { get; set; }

        [Column("old_examination_id")]
        public decimal OldExaminationId { get; set; }

        [Column("new_examination_id")]
        public decimal NewExaminationId { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }
    }
}
