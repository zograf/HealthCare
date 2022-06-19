using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{
    [Table("doctor")]
    public class Doctor
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

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        [Column("specialization_id")]
        public decimal SpecializationId { get; set; }

        public List<Operation> Operations { get; set; }

        public List<Examination> Examinations { get; set; }

        public Credentials Credentials { get; set; }

        public Specialization? Specialization { get; set; }
    }
}
