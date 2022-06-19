using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Models
{
    public class SplitRenovationDomainModel : RenovationDomainModel
    {
        public decimal SplitRoomId { get; set; }
        public decimal ResultRoomId1 { get; set; }
        public decimal ResultRoomId2 { get; set; }
    }
}
