using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiAuthentication.ClientWithAPI.Models;

namespace WebApiAuthentication.ClientWithAPI.Controllers;

public class AccountController : Controller
{
    public IActionResult Login()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (model.Username == "Kevin" && model.Password == "1234")
        {
            List<Claim> claims = new List<Claim>()
            { 
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.Country, "Belgium"),
                new Claim(ClaimTypes.Email, "kevin.dockx@gmail.com")
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            AuthenticationProperties authenticationProperties = new AuthenticationProperties
            {
                /*Via this object, we can pass through a set of properties.
                These have to do with when a cookie expires, whether it can be refreshed, and so on.*/

                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                claimsPrincipal, authenticationProperties);
            
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }

    [Authorize]
    public async Task Logout()
    {
        await HttpContext.SignOutAsync();
    }
}
