using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.DTOs
{
    public class DrugSuggestionUpdateDTO
    {
        public decimal DrugId { get; set; }
        public string Comment { get; set; }
        public string State { get; set; }
    }
}
