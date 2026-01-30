using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using WebApiAuthentication.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddTransient<IClaimsTransformation, DemoClaimsTransformation>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(option =>
    {
        option.MapInboundClaims = false;
    });  

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();