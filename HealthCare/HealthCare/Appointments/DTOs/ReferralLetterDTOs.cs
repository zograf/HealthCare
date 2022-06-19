using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.DTOs
{
    public class CreateAppointmentDTO
    {
        public DateTime StartTime { get; set; }
        public decimal ReferralId { get; set; }
    }
}
