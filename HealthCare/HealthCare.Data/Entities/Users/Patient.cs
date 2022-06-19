using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{
    [Table("patient")]
    public class Patient
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("surname")]
        public string Surname { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("date_of_birth")]
        public DateTime BirthDate { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("blocked")]
        public string? BlockedBy { get; set; }

        [Column("blocking_counter")]
        public decimal BlockingCounter { get; set; }

        [Column("notification_minute_offset")]
        public decimal NotificationOffset { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        public List<Operation> Operations { get; set; }

        public List<Examination> Examinations { get; set; }

        public MedicalRecord MedicalRecord { get; set; }
        public Credentials Credentials { get; set; }

    }
}
