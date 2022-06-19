using HealthCare.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Models
{
    public class DoctorStatsDomainModel
    {
        public Doctor Doctor { get; set; }
        public decimal Average { get; set; }
    }
}
