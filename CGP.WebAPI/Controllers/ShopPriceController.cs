using CGP.Application.Interfaces;
using CGP.Contract.DTO.ShopPrice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CGP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopPriceController : ControllerBase
    {
        private readonly IShopPriceService _shopPriceService;

        public ShopPriceController(IShopPriceService shopPriceService)
        {
            _shopPriceService = shopPriceService;
        }

        [HttpGet("GetAllItemInShop")]
        public async Task<IActionResult> GetAllItemInShopPrice()
        {
            var result = await _shopPriceService.GetAllItemShopPrice();
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
    }
}
