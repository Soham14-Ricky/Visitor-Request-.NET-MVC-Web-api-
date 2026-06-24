using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VisitorMVC.Services.Interfaces;
using VisitorMVC.Services.Repositories;

var builder = WebApplication.CreateBuilder(args);
var sessionTimeoutMinutes = builder.Configuration.GetValue<int>("Session:TimeoutInMinutes");
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout =
        TimeSpan.FromMinutes(30);

    options.Cookie.HttpOnly = true;

    options.Cookie.IsEssential = true;
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.HttpContext.Session.GetString("JWToken");

                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }

                return Task.CompletedTask;
            },

            OnChallenge = context =>
            {
                context.HandleResponse();

                if (!context.Response.HasStarted)
                {
                    context.Response.Redirect("/Account/Login");
                }

                return Task.CompletedTask;
            }
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient();

builder.Services.AddScoped<IAuthService,AuthService>();

builder.Services.AddScoped<IVisitorRequestService,VisitorRequestService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.Use(async (context, next) =>
{
    var endpoint = context.GetEndpoint();
    var allowAnonymous = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;

    if (!allowAnonymous)
    {
        var token = context.Session.GetString("JWToken");

        if (string.IsNullOrEmpty(token))
        {
            context.Response.Redirect("/Account/Login");
            return;
        }

        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(token))
        {
            context.Session.Clear();
            context.Response.Redirect("/Account/Login");
            return;
        }

        var jwt = handler.ReadJwtToken(token);

        if (jwt.ValidTo <= DateTime.UtcNow)
        {
            context.Session.Clear();
            context.Response.Redirect("/Account/Login");
            return;
        }

        var loginTimeString = context.Session.GetString("LoginTime");
        if (!string.IsNullOrEmpty(loginTimeString) &&
            DateTime.TryParse(loginTimeString, out var loginTime))
        {
            if (DateTime.UtcNow - loginTime > TimeSpan.FromMinutes(sessionTimeoutMinutes))
            {
                context.Session.Clear();
                context.Response.Redirect("/Account/Login");
                return;
            }
        }
    }

    await next();
});

app.Use(async (context, next) =>
{

    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";
    await next();
});
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern:
    "{controller=Account}/{action=Login}/{id?}");

app.Run();