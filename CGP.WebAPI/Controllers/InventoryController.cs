using CGP.Application.Interfaces;
using CGP.Contract.DTO.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet("GetInventoryByUserId/{userId}")]
        public async Task<IActionResult> GetByUserIdAsync(Guid userId)
        {
            var result = await _inventoryService.GetByUserIdAsync(userId);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("GetInventoryById/{inventoryId}")]
        public async Task<IActionResult> GetInventoryById(Guid inventoryId)
        {
            var result = await _inventoryService.GetInventoryById(inventoryId);
            if (result.Error != 0)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPost("AddToInventory")]
        public async Task<IActionResult> AddToInventoryAsync([FromBody] AddToInventoryDTO addToInventoryDTO)
        {
            var result = await _inventoryService.AddToInventoryAsync(addToInventoryDTO);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("UpdateInventory")]
        public async Task<IActionResult> UpdateInventoryAsync([FromBody] UpdateInventoryDTO updateInventoryDTO)
        {
            var result = await _inventoryService.UpdateInventoryAsync(updateInventoryDTO);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("DeleteInventory/{inventoryId}")]
        public async Task<IActionResult> DeleteInventoryAsync(Guid inventoryId)
        {
            var result = await _inventoryService.DeleteInventoryAsync(inventoryId);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
