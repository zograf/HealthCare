using HealthCare.Data.Entities;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using HealthCare.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // https://localhost:7195/api/notification
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationDomainModel>>> GetAll()
        {
            try
            {
                IEnumerable<NotificationDomainModel> notifications = await _notificationService.GetAll();
                return Ok(notifications);
            }
            catch (Exception exception)
            {
                return NotFound(exception.Message);
            }
        }

        [HttpPut]
        [Route("send")]
        public async Task<ActionResult<NotificationDomainModel>> Send(SendNotificationDTO dto)
        {
            NotificationDomainModel notification = await _notificationService.Send(dto);
            return Ok(notification);
        }
    }
}