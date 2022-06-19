using System.Diagnostics.Eventing.Reader;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareAPI.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase 
    {
        private IDoctorService _doctorService;
        private ISurveyService _surveyService;

        public DoctorController(IDoctorService doctorService, ISurveyService surveyService) 
        {
            _doctorService = doctorService;
            _surveyService = surveyService;
        }

        // https://localhost:7195/api/doctor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDomainModel>>> GetAll() 
        {
            IEnumerable<DoctorDomainModel> doctors = await _doctorService.GetAll();
            return Ok(doctors);
        }
        
        [HttpGet]
        [Route("read")]
        public async Task<ActionResult<IEnumerable<DoctorDomainModel>>> ReadAll() 
        {
            IEnumerable<DoctorDomainModel> doctors = await _doctorService.ReadAll();
            return Ok(doctors);
        }
        // FOR TESTING
        // DELETE LATER
        [HttpGet]
        [Route("testSchedule/{doctorId}/{duration?}")]
        public async Task<ActionResult<IEnumerable<KeyValuePair<DateTime, DateTime>>>> GetAvailableSchedule(decimal doctorId, decimal duration=15) 
        {
            IEnumerable<KeyValuePair<DateTime, DateTime>> schedule = await _doctorService.GetAvailableSchedule(doctorId, duration);
            return Ok(schedule);
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<DoctorDomainModel>>> SearchDoctors([FromQuery] SearchDoctorsDTO dto)
        {
            IEnumerable<DoctorDomainModel> doctors = await _doctorService.Search(dto);
            return Ok(doctors);
        }

        [HttpGet]
        [Route("doctorId={id}")]
        public async Task<ActionResult<DoctorDomainModel>> GetById(decimal id)
        {
            try
            {
                DoctorDomainModel doctorModel = await _doctorService.GetById(id);
                return Ok(doctorModel);
            }
            catch (Exception exception)
            {
                return NotFound(exception.Message);
            }
        }
    }
}
