using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Models
{
    public enum DaysOffRequestState
    {
        CREATED, 
        APPROVED,
        REJECTED
    }

    public class DaysOffRequestDomainModel
    {
        public decimal Id { get; set; }

        public DaysOffRequestState State { get; set; }

        public string? Comment { get; set; }

        public bool IsUrgent { get; set; }

        public string? RejectionReason { get; set; }

        public decimal DoctorId { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
