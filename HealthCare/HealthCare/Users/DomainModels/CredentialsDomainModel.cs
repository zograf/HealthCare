using HealthCare.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Models 
{
    public class CredentialsDomainModel 
    {
        public decimal Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public decimal? DoctorId { get; set; }

        public decimal? SecretaryId { get; set; }

        public decimal? ManagerId { get; set; }

        public decimal? PatientId { get; set; }

        public decimal UserRoleId { get; set; }
        
        public bool IsDeleted { get; set; }

        public UserRoleDomainModel UserRole { get; set; }
    }
}
