using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
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
        [HttpPost("register")]
        public async Task<IActionResult> Register(RequestValue<string> email)
        {
            try
            {
                await _authService.InitRegistration(email.Value);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            return Ok();
        }
        [HttpPost("submitCode")]
        public async Task<IActionResult> SubmitCode(LoginModel emailAndCode)
        {
            try
            {
                if (await _authService.VerifyCode(emailAndCode.Login, emailAndCode.Password) == false)
                {
                    return BadRequest(new {message = "Wrong code" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            return Ok();
        }
        [HttpPost("finishRegistration")]
        public async Task<IActionResult> FinishRegistration(LoginModel loginModel)
        {
            try
            {
                await _authService.FinishRegistration(loginModel.Login, loginModel.Password);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            return Ok();
        }
    }
}
