using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{
    [Table("examination")]
    public class Examination
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public decimal Id { get; set; }

        [Column("doctor_id")]
        public decimal DoctorId { get; set; }

        [Column("room_Id")]
        public decimal RoomId { get; set; }

        [Column("patient_id")]
        public decimal PatientId { get; set; }

        [Column("examination_started")]
        public DateTime StartTime { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        [Column("emergency")]
        public bool IsEmergency { get; set; }
        public Anamnesis? Anamnesis { get; set; }

    }
}
