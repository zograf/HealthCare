using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{
    [Table("operation")]
    public class Operation
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        [Column("time_held")]
        public DateTime StartTime { get; set; }

        [Column("duration")]
        public decimal Duration { get; set; }

        [Column("room_id")]
        public decimal RoomId  { get; set; }   

        [Column("doctor_id")]
        public decimal DoctorId { get; set; }

        [Column("patient_id")]
        public decimal PatientId { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }
        
        [Column("emergency")]
        public bool IsEmergency { get; set; }
    }
}
