using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.DTOs
{
    public class CreateAnamnesisDTO
    {
        public string Description { get; set; }
        public decimal ExaminationId { get; set; }
    }
}
