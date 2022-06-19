using HealthCare.Domain.DTOs;
using HealthCare.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendExaminationController : ControllerBase
    {
        IRecommendExaminationService _recommendExaminationService;

        public RecommendExaminationController(IRecommendExaminationService recommendExaminationService)
        {
            _recommendExaminationService = recommendExaminationService;
        }

        [HttpPost]
        [Route("recommend")]
        public async Task<ActionResult<IEnumerable<CUExaminationDTO>>> RecommendExaminations([FromBody] ParamsForRecommendingFreeExaminationsDTO paramsDTO)
        {
            try
            {
                IEnumerable<CUExaminationDTO> recommendedExaminations = await _recommendExaminationService.GetRecommendedExaminations(paramsDTO);
                return Ok(recommendedExaminations);
            }
            catch (Exception exception)
            {
                return NotFound(exception.Message);
            }
        }

    }
}
