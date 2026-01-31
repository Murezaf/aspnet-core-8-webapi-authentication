using WebApiAuthentication.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer();

//builder.Services.AddAuthorization(AuthorizationOptions =>
//{
//    AuthorizationOptions.AddPolicy("MustBeGoldAndOlderThan21", policyBuilder =>
//    {
//        policyBuilder.RequireAuthenticatedUser();
//        policyBuilder.RequireClaim("subscriptionlevel", "gold");
//        policyBuilder.RequireAssertion(context =>
//        {
//            var ageClaim = context.User.FindFirst(c => c.Type == "age");
//            if(ageClaim != null && int.TryParse(ageClaim.Value, out var age))
//            {
//                return age > 21;
//            }

//            return false; 
//        });
//    });
//});

builder.Services.AddAuthorization(AuthorizationOptions =>
{
    AuthorizationOptions.AddPolicy(AuthorizationPolicies.MustHaveGoldSubscriptionAndBeOver21, AuthorizationPolicies.MustBeGoldAndOlderThan21());
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();