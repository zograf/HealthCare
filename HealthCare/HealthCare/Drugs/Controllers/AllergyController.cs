using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AllergyController : Controller
    {
        private IAllergyService _allergyService;
        public AllergyController(IAllergyService allergyService)
        {
            _allergyService = allergyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllergyDomainModel>>> GetAll()
        {
            IEnumerable<AllergyDomainModel> anamnesis = await _allergyService.GetAll();
            return Ok(anamnesis);
        }

        [HttpGet]
        [Route("byPatient")]
        public async Task<ActionResult<IEnumerable<AllergyDomainModel>>> GetAllForPatient([FromQuery] decimal patientId)
        {
            IEnumerable<AllergyDomainModel> allergy = await _allergyService.GetAllForPatient(patientId);
            return Ok(allergy);
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<AllergyDomainModel>> Create([FromQuery] AllergyDTO allergyDTO)
        {
            AllergyDomainModel allergy = await _allergyService.Create(allergyDTO);
            return Ok(allergy);
        }

        [HttpPut]
        [Route("delete")]
        public async Task<ActionResult<AllergyDomainModel>> Delete([FromQuery] AllergyDTO allergyDTO)
        {
            AllergyDomainModel allergy = await _allergyService.Delete(allergyDTO);
            return Ok(allergy);
        }
    }
}
