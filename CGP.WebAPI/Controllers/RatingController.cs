using CGP.Application.Interfaces;
using CGP.Contract.DTO.Rating;
using CGP.Contracts.Abstractions.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet("GetRatingsByUserId/{userId}")]
        //[Authorize(Policy = "UserPolicy")] 
        public async Task<IActionResult> GetRatingsByUserId(Guid userId, int pageIndex = 0, int pageSize = 10)
        {
            var result = await _ratingService.GetRatingsByUserId(userId, pageIndex, pageSize);
            if (result.Error == 1)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetRatingsByArtisanId/{artisanId}")]
/*        [Authorize(Policy = "UserPolicy")]*/
        public async Task<IActionResult> GetRatingsByArtisanId(Guid artisanId, int pageIndex = 0, int pageSize = 10)
        {
            var result = await _ratingService.GetRatingsByArtisanId(artisanId, pageIndex, pageSize);
            if (result.Error == 1)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetRatingsByProductId/{productId}")]
        public async Task<IActionResult> GetRatingsByProductId(Guid productId, int pageIndex = 0, int pageSize = 10, int? star = null)
        {
            var result = await _ratingService.GetRatingsByProductId(productId, pageIndex, pageSize, star);
            if (result.Error == 1)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpPost("RatingProduct")]
/*        [Authorize(Policy = "UserPolicy")]*/
        public async Task<IActionResult> RateProduct([FromBody] RatingDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Result<object>
                {
                    Error = 1,
                    Message = "Dữ liệu không hợp lệ.",
                    Data = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }
            var result = await _ratingService.RatingProduct(dto);
            if (result.Error == 1)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

    }
}
