using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Models
{
    public enum DrugSuggestionState
    {
        CREATED,
        REVISION,
        APPROVED,
        REJECTED,
        FOR_REVISION
    }
    public class DrugSuggestionDomainModel
    {
        public decimal Id { get; set; }

        public decimal DrugId { get; set; }

        public DrugSuggestionState State { get; set; }

        public string? Comment { get; set; }

        public DrugDomainModel Drug { get; set; }
    }
}
