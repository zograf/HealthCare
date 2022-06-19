using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Data.Entities
{
    [Table("drug_suggestion")]
    public class DrugSuggestion
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public decimal Id { get; set; }

        [Column("drug_id")]
        public decimal DrugId{ get; set; }

        [Column("state")]
        public string State { get; set; }

        [Column("comment")]
        public string? Comment{ get; set; }

        public Drug Drug { get; set; }
    }
}
