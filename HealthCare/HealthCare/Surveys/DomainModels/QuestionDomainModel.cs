using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Models
{
    public class QuestionDomainModel
    {
        public decimal Id { get; set; }
        public string Text { get; set; }

        public bool IsForDoctor { get; set; }
    }
}
