using System.Diagnostics.Eventing.Reader;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareAPI.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class CredentialsController : ControllerBase 
    {
        private ICredentialsService _credentialsService;

        public CredentialsController(ICredentialsService credentialsService)
        {
            _credentialsService = credentialsService;
        }

        // https://localhost:7195/api/credentials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CredentialsDomainModel>>> GetAll()
        {
            IEnumerable<CredentialsDomainModel> credentials = await _credentialsService.GetAll();
            return Ok(credentials);
        }
        
        [HttpGet]
        [Route("read")]
        public async Task<ActionResult<IEnumerable<CredentialsDomainModel>>> ReadAll()
        {
            IEnumerable<CredentialsDomainModel> credentials = await _credentialsService.ReadAll();
            return Ok(credentials);
        }

        // https://localhost:7195/api/credentials/login/someuser/somepass
        [HttpGet]
        [Route("login/")]
        public async Task<ActionResult<CredentialsDomainModel>> GetLoggedUser([FromQuery] LoginDTO dto)
        {
            try
            {
                CredentialsDomainModel credentials =
                    await _credentialsService.GetCredentialsByUsernameAndPassword(dto);
                return Ok(credentials);
            }
            catch (Exception exception)
            {
                return NotFound(exception.Message);
            }
        }
    }
}
