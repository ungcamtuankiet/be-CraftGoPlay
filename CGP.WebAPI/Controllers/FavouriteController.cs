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
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetCraftVillageById(Guid userId)
        {
            var result = await _favouriteService.GetFavourites(userId);
            if (result.Error != 0)
            {
                return NotFound(result.Message);
            }
            return Ok(result);
        }

        [HttpGet("CheckFavourite")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CheckFavourite([FromQuery] Guid userId, [FromQuery] Guid productId)
        {
            var result = await _favouriteService.CheckFavourite(userId, productId);
            return Ok(new { isFavorited = result.Data });
        }

        [HttpPost("AddFavourite")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddFavourite([FromBody] CreateFavouriteDTO favourite)
        {
            var result = await _favouriteService.AddFavourite(favourite);
            return Ok(result);
        }

        [HttpDelete("DeleteFavourite")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteFavourite([FromQuery] Guid userId, [FromQuery] Guid productId)
        {
            var result = await _favouriteService.DeleteFavouriteByUserAndProduct(userId, productId);
            return Ok(result);
        }
    }
}
