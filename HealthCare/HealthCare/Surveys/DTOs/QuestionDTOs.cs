using HealthCare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.DTOs
{
    public class HospitalQuestionDTO
    {
        public AnswerDomainModel answer1 { get; set; }
        public AnswerDomainModel answer2 { get; set; }
        public AnswerDomainModel answer3 { get; set; }
        public AnswerDomainModel answer4 { get; set; }
    }

    public class DoctorQuestionDTO
    {
        public AnswerDomainModel answer1 { get; set; }
        public AnswerDomainModel answer2 { get; set; }
    }
}
