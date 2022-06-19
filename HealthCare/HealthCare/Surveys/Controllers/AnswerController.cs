using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnswerController : ControllerBase
    {
        private IAnswerService _answerService;

        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerDomainModel>>> GetAll()
        {
            IEnumerable<AnswerDomainModel> answers = await _answerService.GetAll();
            return Ok(answers);
        }

        [HttpGet]
        [Route("forDoctor/{id}")]
        public async Task<ActionResult<IEnumerable<AnswerDomainModel>>> GetForDoctor(decimal id)
        {
            IEnumerable<AnswerDomainModel> answers = await _answerService.GetForDoctor(id);
            return Ok(answers);
        }

        [HttpGet]
        [Route("forHospital")]
        public async Task<ActionResult<IEnumerable<AnswerDomainModel>>> GetForHospital()
        {
            IEnumerable<AnswerDomainModel> answers = await _answerService.GetForHospital();
            return Ok(answers);
        }

        [HttpPost]
        [Route("rateHospital")]
        public async Task<ActionResult<HospitalQuestionDTO>> RateHospital([FromBody] HospitalQuestionDTO dto)
        {
            HospitalQuestionDTO questions = _answerService.RateHospital(dto);
            return Ok(questions);
        }

        [HttpPost]
        [Route("rateDoctor")]
        public async Task<ActionResult<DoctorQuestionDTO>> RateDoctor([FromBody] DoctorQuestionDTO dto)
        {
            DoctorQuestionDTO questions = _answerService.RateDoctor(dto);
            return Ok(questions);
        }
    }
}
