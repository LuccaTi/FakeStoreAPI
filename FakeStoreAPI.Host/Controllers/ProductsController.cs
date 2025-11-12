using FakeStoreAPI.Host.Logging;
using FakeStoreAPI.Host.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FakeStoreAPI.Host.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController : ControllerBase
    {
        #region Atributes
        private const string _className = "ProductsController";
        #endregion

        #region Dependencies
        private readonly IProductService _productService;
        #endregion

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var products = await _productService.GetAllAsync();
                return Ok(products);
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
                var product = await _productService.GetByIdAsync(id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "GetByIdAsync", $"Error processing request: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
