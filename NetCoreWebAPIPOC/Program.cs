using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using NetCoreWebAPIPOC.OAuth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var oktaDomain = "https://dev-85818545.okta.com";

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = oktaDomain + "/oauth2/default";
        options.Audience = "api://default";
        options.RequireHttpsMetadata = true; // Set to false only in development
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = oktaDomain + "/oauth2/default",
            ValidateAudience = true,
            ValidAudience = "api://default", 
            ValidateLifetime = true,
        };
    });


builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("APIOauthPOCScope", policy =>
        policy.Requirements.Add(new HasScopeRequirement("APIOauthPOCScope", "https://dev-85818545.okta.com/oauth2/default")));
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HasCustomRolePOC", policy =>
        policy.RequireClaim("CustomRolePOC"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
