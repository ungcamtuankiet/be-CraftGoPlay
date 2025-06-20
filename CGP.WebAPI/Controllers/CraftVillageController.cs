using CGP.Application.Interfaces;
using CGP.Contract.DTO.CraftVillage;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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

        // Get all craft villages
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

        // Get a craft village by ID
        [HttpGet("GetCraftVillageById/{id}")]
        public async Task<IActionResult> GetCraftVillageById(Guid id)
        {
            var result = await _craftVillageService.GetCraftVillageByIdAsync(id);
            if (result.Error != 0)
            {
                return NotFound(result.Message);
            }
            return Ok(result);
        }

        // Create a new craft village
        [HttpPost("CreateNewCraftVillage")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Result<CraftVillage>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<CraftVillage>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> CreateNewCraftVillage([FromBody] CreateCraftVillageDTO craftVillage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Result<CraftVillage>
                {
                    Error = 1,
                    Message = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                });
            }

            var result = await _craftVillageService.CreateNewCraftVillage(craftVillage);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(nameof(GetCraftVillageById), new { id = result.Data.Id }, result);
        }

        // Update an existing craft village
        [HttpPut("UpdateCraftVillage/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<CraftVillage>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<CraftVillage>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateCraftVillage(Guid id, [FromBody] UpdateCraftVillageDTO craftVillage)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new Result<CraftVillage>
                {
                    Error = 1,
                    Message = "Invalid craft village ID."
                });
            }

            if (craftVillage.Id != id)
            {
                return BadRequest(new Result<CraftVillage>
                {
                    Error = 1,
                    Message = "The ID in the DTO does not match the provided ID."
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new Result<CraftVillage>
                {
                    Error = 1,
                    Message = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                });
            }

            var result = await _craftVillageService.UpdateCraftVillage(id, craftVillage);
            if (result.Error != 0)
            {
                return result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase)
                    ? NotFound(result)
                    : BadRequest(result);
            }

            return Ok(result);
        }

        // Delete a craft village
        [HttpDelete("DeleteCraftVillage/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<bool>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Result<bool>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> DeleteCraftVillage(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new Result<bool>
                {
                    Error = 1,
                    Message = "Invalid craft village ID."
                });
            }

            var result = await _craftVillageService.DeleteCraftVillage(id);
            if (result.Error != 0)
            {
                return result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase)
                    ? NotFound(result)
                    : BadRequest(result);
            }

            return NoContent();
        }
    }
}