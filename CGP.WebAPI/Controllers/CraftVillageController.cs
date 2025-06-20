using CGP.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CraftVillageController : ControllerBase
    {
        private readonly ICraftVillageService _craftVillageService;
        public CraftVillageController(ICraftVillageService craftVillageService)
        {
            _craftVillageService = craftVillageService;
        }
        [HttpGet("GetAllCraftVillages")]
        public async Task<IActionResult> GetAllCraftVillages()
        {
            var result = await _craftVillageService.GetAllCraftVillagesAsync();
            if (result.Error != 0)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
