using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{
    [Table("answer")]
    public class Answer
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        [Column("answer_text")]
        public string AnswerText { get; set; }

        [Column("evaluation")]
        public decimal Evaluation{ get; set; }

        [Column("question_id")]
        public decimal QuestionId { get; set; }

        [Column("patient_id")]
        public decimal PatientId { get; set; }

        [Column("doctor_id")]
        public decimal? DoctorId { get; set; }

        public Question Question{ get; set; }

        public Patient Patient { get; set; }

        public Doctor Doctor { get; set; }
    }
}
