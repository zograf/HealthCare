using System.Diagnostics.Eventing.Reader;
using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientController : ControllerBase
    {
        private IIngredientService _ingridientService;

        public IngredientController(IIngredientService ingridientService)
        {
            _ingridientService = ingridientService;
        }

        // https://localhost:7195/api/ingridient
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientDomainModel>>> GetAll()
        {
            IEnumerable<IngredientDomainModel> ingridients = await _ingridientService.GetAll();
            return Ok(ingridients);
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<IngredientDomainModel>> Create([FromBody] IngredientDTO dto)
        {
            IngredientDomainModel ingredient = _ingridientService.Create(dto);
            return Ok(ingredient);
        }

        [HttpPut]
        [Route("delete/id={id}")]
        public async Task<ActionResult<IngredientDomainModel>> Delete(decimal id)
        {
            IngredientDomainModel ingredient = await _ingridientService.Delete(id);
            return Ok(ingredient);
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<IngredientDomainModel>> Update([FromBody] IngredientDTO dto)
        {
            IngredientDomainModel ingredient = _ingridientService.Update(dto);
            return Ok(ingredient);
        }

    }
}
