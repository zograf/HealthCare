using System.Diagnostics.Eventing.Reader;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareAPI.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecretaryController : ControllerBase 
    {
        private ISecretaryService _secretaryService;

        public SecretaryController(ISecretaryService secretaryService) 
        {
            _secretaryService = secretaryService;
        }

        // https://localhost:7195/api/secretary
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SecretaryDomainModel>>> GetAll() 
        {
            IEnumerable<SecretaryDomainModel> secretaries = await _secretaryService.GetAll();
            return Ok(secretaries);
        }
        
        [HttpGet]
        [Route("read")]
        public async Task<ActionResult<IEnumerable<SecretaryDomainModel>>> ReadAll() 
        {
            IEnumerable<SecretaryDomainModel> secretaries = await _secretaryService.ReadAll();
            return Ok(secretaries);
        }
        
        [HttpGet]
        [Route("secretaryId={id}")]
        public async Task<ActionResult<SecretaryDomainModel>> GetById(decimal id) 
        {
            SecretaryDomainModel secretary = await _secretaryService.GetById(id);
            return Ok(secretary);
        }
    }
}
