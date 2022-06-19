using System.Diagnostics.Eventing.Reader;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareAPI.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserRoleController : ControllerBase 
    {
        private IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService) 
        {
            _userRoleService = userRoleService;
        }

        // https://localhost:7195/api/userRole
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRoleDomainModel>>> GetAll() 
        {
            IEnumerable<UserRoleDomainModel> userRoles = await _userRoleService.GetAll();
            return Ok(userRoles);
        }
        [HttpGet]
        [Route("read")]
        public async Task<ActionResult<IEnumerable<UserRoleDomainModel>>> ReadAll() 
        {
            IEnumerable<UserRoleDomainModel> userRoles = await _userRoleService.ReadAll();
            return Ok(userRoles);
        }
    }
}
