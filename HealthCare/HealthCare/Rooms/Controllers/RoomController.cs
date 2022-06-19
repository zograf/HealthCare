using System.Diagnostics.Eventing.Reader;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        // https://localhost:7195/api/room
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDomainModel>>> GetAll()
        {
            IEnumerable<RoomDomainModel> rooms = await _roomService.GetAll();
            return Ok(rooms);
        }
        
        [HttpGet]
        [Route("read")]
        public async Task<ActionResult<IEnumerable<RoomDomainModel>>> ReadAll()
        {
            IEnumerable<RoomDomainModel> rooms = await _roomService.ReadAll();
            return Ok(rooms);
        }
        // https://localhost:7195/api/room/create
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<RoomDomainModel>> CreateRoom([FromBody] CURoomDTO dto)
        {
            try
            {
                RoomDomainModel insertedRoomModel = await _roomService.Create(dto);
                return Ok(insertedRoomModel);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // https://localhost:7195/api/room/update
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<RoomDomainModel>> UpdateRoom([FromBody] CURoomDTO dto)
        {
            try
            {
                RoomDomainModel updatedRoomModel = await _roomService.Update(dto);
                return Ok(updatedRoomModel);
            }
            catch (Exception exception)
            {
                return NotFound(exception.Message);
            }
        }

        // https://localhost:7195/api/room/delete
        [HttpPut]
        [Route("delete/{id}")]
        public async Task<ActionResult<RoomDomainModel>> DeleteRoom(decimal id) 
        {
            try
            {
                RoomDomainModel deletedRoomModel = await _roomService.Delete(id);
                return Ok(deletedRoomModel);
            }
            catch (Exception exception)
            {
                return NotFound(exception.Message);
            }
        }
    }
}
