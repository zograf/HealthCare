using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.DTOs
{
    public class CreateDaysOffRequestDTO
    {
        public string Comment { get; set; }

        public bool IsUrgent { get; set; }

        public decimal DoctorId { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }

    public class RejectDaysOffRequestDTO
    {
        public decimal Id { get; set; }
        
        public string Comment { get; set; }
    }
}
