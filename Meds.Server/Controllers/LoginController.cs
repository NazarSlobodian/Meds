using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Meds.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly AuthService _authService;
        public LoginController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            RoleData role = await _authService.AutheticateUserAsync(loginModel.Login, loginModel.Password);
            if (role == null)
            {
                return Unauthorized(new { message = "Invalid" });
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, role.RoleName),
                new Claim("UserID", role.AccountId.ToString())
            };
            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CookieAuth", principal);
            return Ok(new { role = role.RoleName, userId = role.AccountId });
        }
    }
}
