using System.Diagnostics.Eventing.Reader;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareAPI.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExaminationApprovalController : ControllerBase 
    {
        private IExaminationApprovalService _examinationApprovalService;

        public ExaminationApprovalController(IExaminationApprovalService examinationApprovalService) 
        {
            _examinationApprovalService = examinationApprovalService;
        }

        // https://localhost:7195/api/examinationApproval
        [HttpGet]
        [Route("read")]
        public async Task<ActionResult<IEnumerable<ExaminationApprovalDomainModel>>> ReadAll() 
        {
            IEnumerable<ExaminationApprovalDomainModel> examinationApprovals = await _examinationApprovalService.ReadAll();
            return Ok(examinationApprovals);
        }
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<ExaminationApprovalDomainModel>>> GetAll() 
        {
            IEnumerable<ExaminationApprovalDomainModel> examinationApprovals = await _examinationApprovalService.GetAll();
            return Ok(examinationApprovals);
        }
        
        // https://localhost:7195/api/examinationApproval/reject
        [HttpPut]
        [Route("reject/{id}")]
        public async Task<ActionResult<IEnumerable<ExaminationApprovalDomainModel>>> Reject(decimal id) 
        {
            try
            {
                ExaminationApprovalDomainModel examinationApproval = await _examinationApprovalService.Reject(id);
                return Ok(examinationApproval);
            }
            catch (Exception exception)
            {
                return NotFound(exception.Message);
            }
        }
        
        // https://localhost:7195/api/examinationApproval/approve
        [HttpPut]
        [Route("approve/{id}")]
        public async Task<ActionResult<IEnumerable<ExaminationApprovalDomainModel>>> Approve(decimal id) 
        {
            try
            {
                ExaminationApprovalDomainModel examinationApproval = await _examinationApprovalService.Approve(id);
                return Ok(examinationApproval);
            }
            catch (Exception exception)
            {
                return NotFound(exception.Message);
            }
        }
    }
}
