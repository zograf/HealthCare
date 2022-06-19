using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{
    [Table("creds")]
    public class Credentials
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("Doctor_id")]

        public decimal? DoctorId { get; set; }

        [Column("Secretary_Id")]
        public decimal? SecretaryId { get; set; }

        [Column("Manager_id")] 
        public decimal? ManagerId { get; set; }
        
        [Column("Patient_id")]
        public decimal? PatientId { get; set; }

        [Column("User_role_id")]
        public decimal UserRoleId { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }

        public UserRole UserRole { get; set; }
    }
}
