using HealthCare.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Models
{
    public class AnswerStatsDomainModel
    {
        public int Count { get; set;}
        public decimal Average { get; set;}
        public IEnumerable<string> Comments { get; set;}

        public Question question { get; set; }
    }
}
