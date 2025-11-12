using FakeStoreAPI.Host.Services.Interfaces;
using FakeStoreAPI.Host.Logging;
using Microsoft.AspNetCore.Mvc;

namespace FakeStoreAPI.Host.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        #region Atributes
        private const string _className = "UsersController";
        #endregion

        #region Dependencies
        private IUserService _userService;
        #endregion

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                return Ok(users);
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
                var user = await _userService.GetByIdAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                Logger.Error(_className, "GetByIdAsync", $"Error processing request: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
