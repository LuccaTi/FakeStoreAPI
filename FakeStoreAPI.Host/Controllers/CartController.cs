using FakeStoreAPI.Host.Services.Interfaces;
using FakeStoreAPI.Host.Logging;
using Microsoft.AspNetCore.Mvc;

namespace FakeStoreAPI.Host.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {
        #region Atributes
        private const string _className = "CartController";
        private readonly ICartService _cartService;
        #endregion

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var carts = await _cartService.GetAllAsync();
                return Ok(carts);
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "GetAllAsync", $"Error processing request: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            try
            {
                var cart = await _cartService.GetByIdAsync(id);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "GetByIdAsync", $"Error processing request: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
