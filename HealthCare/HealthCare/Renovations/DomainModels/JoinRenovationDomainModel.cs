using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Models
{
    public class JoinRenovationDomainModel : RenovationDomainModel
    {
        public decimal ResultRoomId { get; set; }
        public decimal JoinRoomId1 { get; set; }
        public decimal JoinRoomId2 { get; set; }
    }
}
