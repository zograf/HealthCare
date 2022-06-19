using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.DTOs
{
    public class FilterEquipmentDTO
    {
        public decimal? EquipmentTypeId { get; set; }
        public int? MinAmount { get; set; }
        public int? MaxAmount { get; set; }
        public decimal? RoomTypeId { get; set; }
    }
}
