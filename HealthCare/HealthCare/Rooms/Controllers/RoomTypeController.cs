using System.Diagnostics.Eventing.Reader;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareAPI.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomTypeController : ControllerBase 
    {
        private IRoomTypeService _roomTypeService;

        public RoomTypeController(IRoomTypeService roomTypeService) 
        {
            _roomTypeService = roomTypeService;
        }

        // https://localhost:7195/api/roomType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomTypeDomainModel>>> GetAll() 
        {
            IEnumerable<RoomTypeDomainModel> roomTypes = await _roomTypeService.GetAll();
            return Ok(roomTypes);
        }
        [HttpGet]
        [Route("read")]
        public async Task<ActionResult<IEnumerable<RoomTypeDomainModel>>> ReadAll() 
        {
            IEnumerable<RoomTypeDomainModel> roomTypes = await _roomTypeService.ReadAll();
            return Ok(roomTypes);
        }
    }
}
