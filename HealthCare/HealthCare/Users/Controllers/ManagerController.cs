using System.Diagnostics.Eventing.Reader;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareAPI.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagerController : ControllerBase 
    {
        private IManagerService _managerService;

        public ManagerController(IManagerService managerService) 
        {
            _managerService = managerService;
        }

        // https://localhost:7195/api/manager
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ManagerDomainModel>>> GetAll() 
        {
            IEnumerable<ManagerDomainModel> managers = await _managerService.GetAll();
            return Ok(managers);
        }

        [HttpGet]
        [Route("read")]
        public async Task<ActionResult<IEnumerable<ManagerDomainModel>>> ReadAll()
        {
            IEnumerable<ManagerDomainModel> managers = await _managerService.ReadAll();
            return Ok(managers);
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IEnumerable<ManagerDomainModel>>> GetById(decimal id) 
        {
            ManagerDomainModel manager = await _managerService.GetById(id);
            return Ok(manager);
        }
    }
}
