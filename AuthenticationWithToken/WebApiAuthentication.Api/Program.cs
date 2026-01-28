using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            //What will be validated on the incoming token:
            ValidateIssuer = true, //For example: We're going to validate the issuer, so I set that to true.
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            //The expiration time of the token is automatically validated. If the token is expired, the client must request a new token to be able to continue.

            //Now we need to tell this middleware where it can find the correct values.
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(builder.Configuration["Authentication:SecretForKey"] ?? throw new KeyNotFoundException("Secretkey not found or invalid.")))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
