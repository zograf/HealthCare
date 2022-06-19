using System.Diagnostics.Eventing.Reader;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareAPI.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnamnesisController : ControllerBase 
    {
        private IAnamnesisService _anamnesisService;

        public AnamnesisController(IAnamnesisService anamnesisService) 
        {
            _anamnesisService = anamnesisService;
        }

        // https://localhost:7195/api/anamnsesis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnamnesisDomainModel>>> GetAll() 
        {
            IEnumerable<AnamnesisDomainModel> anamnesis =  await _anamnesisService.GetAll();
            return Ok(anamnesis);
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<AnamnesisDomainModel>> Create([FromBody] CreateAnamnesisDTO newAnamnesisModel)
        {
            try
            {
                AnamnesisDomainModel anamnesisModel = await _anamnesisService.Create(newAnamnesisModel);
                return Ok(anamnesisModel);
            }
            catch (Exception exception)
            {
                return NotFound(exception.Message);
            }

        }
        
        [HttpGet]
        [Route("read")]
        public async Task<ActionResult<IEnumerable<AnamnesisDomainModel>>> ReadAll() 
        {
            IEnumerable<AnamnesisDomainModel> anamnesis =  await _anamnesisService.ReadAll();
            return Ok(anamnesis);
        }
    }
}
