using CGP.Application.Interfaces;
using CGP.Contract.DTO.ShopPrice;
using CGP.Contracts.Abstractions.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopPriceController : ControllerBase
    {
        private readonly IShopPriceService _shopPriceService;
        private readonly IInventoryService _inventoryService;

        public ShopPriceController(IShopPriceService shopPriceService, IInventoryService inventoryService)
        {
            _shopPriceService = shopPriceService;
            _inventoryService = inventoryService;
        }

        [HttpGet("GetAllItemInShop")]
        public async Task<IActionResult> GetAllItemInShopPrice()
        {
            var result = await _shopPriceService.GetAllItemShopPrice();
            return Ok(result);
        }

        [HttpGet("GetItemsSell")]
        public async Task<IActionResult> GetItemsSell()
        {
            var result = await _shopPriceService.GetItemsSell();
            return Ok(result);
        }

        [HttpGet("GetItemsInBackpackByUserId/{userId}")]
        public async Task<IActionResult> GetItemsInBackpackByUserId(Guid userId)
        {
            var result = await _inventoryService.GetItemsInInventoryTypeAsync(userId);
            return Ok(result);
        }

        [HttpGet("GetItemInShop/{id}")]
        public async Task<IActionResult> GetAllItemInShopPrice(Guid id)
        {
            var result = await _shopPriceService.GetItemShopPrice(id);
            if(result.Error == 1)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("AddItemToShop")]
        public async Task<IActionResult> AddItemToShop([FromForm] CreateItemShopPriceDTO itemShopPriceDTO)
        {
            var result = await _shopPriceService.CreateShopPriceItem(itemShopPriceDTO);
            if (result.Error == 1)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("UpdateItemInShop")]
        public async Task<IActionResult> UpdateItemInShop([FromForm] UpdateItemShopPriceDTO itemShopPriceDTO)
        {
            var result = await _shopPriceService.UpdateShopPriceItem(itemShopPriceDTO);
            if (result.Error == 1)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("RemoveItemInShop/{id}")]
        public async Task<IActionResult> RemoveItemInShop(Guid id)
        {
            var result = await _shopPriceService.RemoveShopPriceItem(id);
            if (result.Error == 1)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("SellItem")]
        public async Task<IActionResult> SellItem([FromForm] SellRequest request)
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
            var result = await _shopPriceService.SellItem(request);
            if (result.Error == 1)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
