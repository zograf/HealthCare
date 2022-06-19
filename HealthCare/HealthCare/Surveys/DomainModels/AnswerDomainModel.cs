using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Models
{
    public class AnswerDomainModel
    {
        public decimal Id { get; set; }
        public string AnswerText { get; set; }
        public decimal Evaluation{ get; set; }
        public decimal? DoctorId{ get; set; }
        public decimal PatientId{ get; set; }
        public decimal QuestionId{ get; set; }
    }
}
