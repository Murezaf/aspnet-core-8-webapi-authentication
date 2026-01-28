using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace WebApiAuthentication.Api.Controllers;

[Route("account")]
[ApiController]
public class AccountController(IConfiguration configuration) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;

    public class AuthenticationRequestBody
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    [HttpPost("login")]
    public ActionResult<string> Login(AuthenticationRequestBody authenticationRequestBody)
    {
        /*In real life, user credentials are probably stored in a database table or in a separate user database. 
          We don't have a user database or table in our application database,
          So what I'm going to do here is assume that the credentials are valid when the username is Kevin and the password is 1234.*/
        if (authenticationRequestBody.Username == "Kevin" && authenticationRequestBody.Password == "1234")
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Convert.FromBase64String(_configuration["Authentication:SecretForKey"] ?? throw new KeyNotFoundException("SecretForKey not found or invalid.")));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            /*A token can contain a variety of claims that contain information, for example, on who the user is. When we're talking about a user, a claim in that context is identity‑related information on that user.
            In reality, these would typically come from database. For the demo, we only allow Kevin, to sign in so hard coded these here.*/
            List<Claim> claimsForToken = new List<Claim>()
            {
                new Claim("sub", "1"), //sub is a standardized key for the unique user identifier.
                new Claim("given_name", "Kevin"),
                new Claim("family_name", "Dockx"),
                new Claim("email", "Kevin.Dockx@gmail.com")
            };

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials
                );

            string tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return Ok(tokenToReturn);
        }

        return Unauthorized();
    }
}
