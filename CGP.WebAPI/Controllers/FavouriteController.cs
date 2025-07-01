using CGP.Application.Interfaces;
using CGP.Application.Services;
using CGP.Contract.DTO.Favourite;
using CGP.Contracts.Abstractions.Shared;
using CGP.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouriteController : ControllerBase
    {
        private readonly IFavouriteService _favouriteService;

        public FavouriteController(IFavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
        }

        [HttpGet("GetFavourites/{userId}")]
        public async Task<IActionResult> GetCraftVillageById(Guid userId)
        {
            var result = await _favouriteService.GetFavourites(userId);
            if (result.Error != 0)
            {
                return NotFound(result.Message);
            }
            return Ok(result);
        }

        [HttpPost("AddFavourite")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Result<Favourite>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<Favourite>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddFavourite([FromBody] CreateFavouriteDTO favourite)
        {
            var result = await _favouriteService.AddFavourite(favourite);
            return Ok(result);
        }

        [HttpDelete("DeleteFavourite/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Result<Favourite>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result<Favourite>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteFavourite(Guid id)
        {
            var result = await _favouriteService.DeleteFavourite(id);
            return Ok(result);
        }
    }
}
