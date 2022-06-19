using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{
    [Table("days_off_request")]
    public class DaysOffRequest
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        [Column("state")]
        public string State { get; set; }

        [Column("comment")]
        public string? Comment { get; set; }

        [Column("emergency")]
        public bool IsUrgent { get; set; }

        [Column("reject_reason")]
        public string? RejectionReason { get; set; }

        [Column("doctor_id")]
        public decimal DoctorId { get; set; }

        [Column("start_date")]
        public DateTime From { get; set; }

        [Column("end_date")]
        public DateTime To { get; set; }
    }
}
