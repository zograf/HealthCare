using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using HealthCare.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrugIngredientController : Controller
    {
        private IDrugIngredientService _drugIngredientService;
        public DrugIngredientController(IDrugIngredientService drugIngredientService)
        {
            _drugIngredientService = drugIngredientService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DrugIngredientDomainModel>>> GetAll()
        {
            IEnumerable<DrugIngredientDomainModel> drugIngredients = await _drugIngredientService.GetAll();
            return Ok(drugIngredients);
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<DrugIngredientDomainModel>> Create([FromQuery] DrugIngredientDTO drugIngredientDTO)
        {
            DrugIngredientDomainModel drugIngredient = _drugIngredientService.Create(drugIngredientDTO);
            return Ok(drugIngredient);
        }

        [HttpPut]
        [Route("delete")]
        public async Task<ActionResult<DrugIngredientDomainModel>> Delete(decimal drugId, decimal ingredientId)
        {
            DrugIngredientDomainModel drugIngredient = await _drugIngredientService.Delete(drugId, ingredientId);
            return Ok(drugIngredient);
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<DrugIngredientDomainModel>> Update([FromQuery] DrugIngredientDTO drugIngredientDTO)
        {
            DrugIngredientDomainModel drugIngredient = _drugIngredientService.Update(drugIngredientDTO);
            return Ok(drugIngredient);
        }
    }
}
