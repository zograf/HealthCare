using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Models 
{
    public class AntiTrollDomainModel 
    {
        public decimal Id { get; set; }
        public string State { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal PatientId { get; set; }
    }
}
