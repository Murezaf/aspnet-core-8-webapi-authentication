using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace WebApiAuthentication.Api;

public class DemoClaimsTransformation : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = principal.Identity as ClaimsIdentity;

        if(identity == null)
        {
            return Task.FromResult(principal);
        }

        var nbfClaim = identity.FindFirst("nbf");
        if(nbfClaim != null)
        {
            identity.RemoveClaim(nbfClaim);
        }

        var iatClaim = identity.FindFirst("iat");
        if (iatClaim != null)
        {
            identity.RemoveClaim(iatClaim);
        }

        //You can't rename a claim directly. What you can do is removing the claim and create another one for that value
        var levelOfAccessClaim = identity.FindFirst("levelofaccess");
        if (levelOfAccessClaim != null)
        {
            identity.RemoveClaim(levelOfAccessClaim);
            identity.AddClaim(new Claim("subscriptionlevel", levelOfAccessClaim.Value));
        }

        return Task.FromResult(principal);
    }
}
