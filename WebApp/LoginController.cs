using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace WebApp
{
    [Route("api")]
    public class LoginController : Controller
    {
        private readonly IAccountDatabase _db;

        public LoginController(IAccountDatabase db)
        {
            _db = db;
        }
        
        [HttpPost("sign-in")]
        [HttpGet("sign-in")]
        public async Task<IActionResult> Login(string userName)
        {
            var account = await _db.FindByUserNameAsync(userName);
            if (account != null)
            {
                //TODO 1: Generate auth cookie for user 'userName' with external id --> DONE
                var claims = new List<Claim>
                {
                    new Claim("external-id", account.ExternalId),
                    new Claim(ClaimTypes.Role, account.Role) // for authorization purposes (TO_DO 7)
                };
                
                var claimsIdentity = new ClaimsIdentity(
                    claims, "ApplicationCookie", "external-id", ClaimTypes.Role);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, 
                    claimsPrincipal);
                return Ok();
            }
            //TODO 2: return 404 not found if user not found --> DONE
            return NotFound();
        }
    }
}