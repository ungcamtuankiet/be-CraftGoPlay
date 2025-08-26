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

        [HttpPost("AddItemToInventory")]
        public async Task<IActionResult> AddItemToInventory([FromForm] AddItemToInventoryDTO addItemToInventoryDTO)
        {
            var result = await _inventoryService.AddItemToInventoryAsync(addItemToInventoryDTO);
            if (result.Error != 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("UpdateInventory")]
        public async Task<IActionResult> UpdateInventoryAsync([FromForm] UpdateInventoryDTO updateInventoryDTO)
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
