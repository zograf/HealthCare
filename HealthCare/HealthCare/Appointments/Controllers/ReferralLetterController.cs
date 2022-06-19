using System.Diagnostics.Eventing.Reader;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Domain.DTOs;
using HealthCare.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReferralLetterController : ControllerBase
    {
        private IReferralLetterService _referralLetterService;

        public ReferralLetterController(IReferralLetterService referralLetterService)
        {
            _referralLetterService = referralLetterService;
        }

        // https://localhost:7195/api/referralLetter
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReferralLetterDomainModel>>> GetAll()
        {
            IEnumerable<ReferralLetterDomainModel> referralLetters = await _referralLetterService.GetAll();
            return Ok(referralLetters);
        }

        [HttpPut]
        [Route("createAppointment")]
        public async Task<ActionResult<IEnumerable<ReferralLetterDomainModel>>> CreateAppointment([FromBody] CreateAppointmentDTO dto)
        {
            try
            {
                ReferralLetterDomainModel referralLetter =
                    await _referralLetterService.CreateAppointment(dto);
                return Ok(referralLetter);
            }
            catch (Exception exception)
            {
                return NotFound(exception.Message);
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<ReferralLetterDomainModel>> Create([FromBody] ReferralLetterDTO referralDTO)
        {
            try
            {
                ReferralLetterDomainModel createdReferralModel = await _referralLetterService.Create(referralDTO);
                return Ok(createdReferralModel);
            }
            catch (Exception exception)
            {
                return NotFound(exception.Message);
            }
        }
    }
}
