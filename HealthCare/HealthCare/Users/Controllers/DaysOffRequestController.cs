using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DaysOffRequestController : Controller
    {
        private IDaysOffRequestService _daysOffRequestService;
        public DaysOffRequestController(IDaysOffRequestService daysOffRequestService)
        {
            _daysOffRequestService = daysOffRequestService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DaysOffRequestDomainModel>>> GetAll()
        {
            IEnumerable<DaysOffRequestDomainModel> daysOff = await _daysOffRequestService.GetAll();
            return Ok(daysOff);
        }

        [HttpGet]
        [Route("byDoctor/{doctorId}")]
        public async Task<ActionResult<IEnumerable<DaysOffRequestDomainModel>>> GetAllForDoctor(decimal doctorId)
        {
            IEnumerable<DaysOffRequestDomainModel> daysOff = await _daysOffRequestService.GetAllForDoctor(doctorId);
            return Ok(daysOff);
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<DaysOffRequestDomainModel>> Create([FromBody] CreateDaysOffRequestDTO daysOffRequest)
        {
            try
            {
                DaysOffRequestDomainModel daysOffRequestModel = await _daysOffRequestService.Create(daysOffRequest);
                return Ok(daysOffRequestModel);
            }
            catch (Exception exception)
            {
                return NotFound(exception.Message);
            }
        }
        
        [HttpPut]
        [Route("approve/{id}")]
        public async Task<ActionResult<DaysOffRequestDomainModel>> Approve(decimal id)
        {
            try
            {
                _ = await _daysOffRequestService.Approve(id);
                return Ok();
            }
            catch (Exception exception)
            {
                return NotFound(exception.Message);
            }
        }
        
        [HttpPut]
        [Route("reject")]
        public async Task<ActionResult<DaysOffRequestDomainModel>> Approve([FromBody] RejectDaysOffRequestDTO dto)
        {
            try
            { 
                _ = await _daysOffRequestService.Reject(dto);
                return Ok();
            }
            catch (Exception exception)
            {
                return NotFound(exception.Message);
            }
        }
    }
}
