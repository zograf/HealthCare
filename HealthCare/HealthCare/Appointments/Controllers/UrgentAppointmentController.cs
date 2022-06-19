using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrgentAppointmentController : Controller
    {
        private IUrgentAppointmentService _urgentAppointmentService;

        public UrgentAppointmentController(IUrgentAppointmentService urgentAppointmentService)
        {
            _urgentAppointmentService = urgentAppointmentService;
        }

        [HttpPut]
        [Route("urgentList")]
        public async Task<ActionResult<IEnumerable<IEnumerable<RescheduleDTO>>>> CreateUrgentAppointment(CreateUrgentAppointmentDTO dto)
        {
            Boolean isCreated = await _urgentAppointmentService.CreateUrgent(dto);
            if (isCreated) return Ok();
            IEnumerable<IEnumerable<RescheduleDTO>> rescheduleItems = await _urgentAppointmentService.FindFiveAppointments(dto);
            return Ok(rescheduleItems);
        }

        [HttpPut]
        [Route("urgent")]
        public async Task<ActionResult<ExaminationDomainModel>> RescheduleForUrgentAppointment(List<RescheduleDTO> dto)
        {
            _ = await _urgentAppointmentService.AppointUrgent(dto);
            return Ok();
        }
    }
}
