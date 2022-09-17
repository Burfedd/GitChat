using GitChat.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GitChat.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AccountsController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterDTO dto)
        {
            var newUser = new IdentityUser
            {
                UserName = dto.Username,
                Email = dto.Email,
            };

            var result = await _userManager.CreateAsync(newUser, dto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return Ok(new RegisterResultDTO { Successful = false, Errors = errors });
            }

            return Ok(new RegisterResultDTO { Successful = true });
        }
    }
}
