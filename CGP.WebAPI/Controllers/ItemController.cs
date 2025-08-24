using CGP.Application.Interfaces;
using CGP.Contract.DTO.Item;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet("GetAllItems")]
        public async Task<IActionResult> GetAllItems()
        {
            var result = await _itemService.GetAllItemsAsync();
            if (result.Error == 1)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetItemById/{id}")]
        public async Task<IActionResult> GetItemById(Guid id)
        {
            var result = await _itemService.GetItemByIdAsync(id);
            if (result.Error == 1)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("CreateItem")]
        public async Task<IActionResult> CreateItem([FromForm] CreateItemDTO createItemDTO)
        {
            var result = await _itemService.CreateItem(createItemDTO);
            if (result.Error == 1)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("UpdateItem")]
        public async Task<IActionResult> UpdateItem([FromForm] UpdateItemDTO updateItemDTO)
        {
            var result = await _itemService.UpdateItem(updateItemDTO);
            if (result.Error == 1)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("DeleteItem/{id}")]
        public async Task<IActionResult> DeleteItem(Guid id)
        {
            var result = await _itemService.DeleteItem(id);
            if (result.Error == 1)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
