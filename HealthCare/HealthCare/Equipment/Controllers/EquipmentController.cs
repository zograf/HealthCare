using System.Diagnostics.Eventing.Reader;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareAPI.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentController : ControllerBase 
    {
        private IEquipmentService _equipmentService;

        public EquipmentController(IEquipmentService equipmentService) 
        {
            _equipmentService = equipmentService;
        }

        // https://localhost:7195/api/equipment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentDomainModel>>> GetAll() 
        {
            IEnumerable<EquipmentDomainModel> equipment = await _equipmentService.GetAll();
            return Ok(equipment);
        }
        
        [HttpGet]
        [Route("read")]
        public async Task<ActionResult<IEnumerable<EquipmentDomainModel>>> ReadAll() 
        {
            IEnumerable<EquipmentDomainModel> equipment = await _equipmentService.ReadAll();
            return Ok(equipment);
        }
        // https://localhost:7195/api/equipment/search
        [HttpGet]
        [Route("search/{substring}")]
        public async Task<ActionResult<IEnumerable<EquipmentDomainModel>>> GetByName(string substring)
        {
            try
            {
                IEnumerable<EquipmentDomainModel> equipment = await _equipmentService.SearchByName(substring);
                return Ok(equipment);
            }
            catch (Exception exception)
            {
                return NotFound(exception.Message);
            }
        }

        [HttpGet]
        [Route("filter")]
        public async Task<ActionResult<IEnumerable<EquipmentDomainModel>>> GetFilteredEquipment([FromQuery]FilterEquipmentDTO dto)
        {
            try
            {
                IEnumerable<EquipmentDomainModel> equipment = await _equipmentService.Filter(dto);
                return Ok(equipment);
            }
            catch (Exception exception)
            {
                return NotFound(exception.Message);
            }
        }
    }
}
