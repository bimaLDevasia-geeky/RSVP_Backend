using Microsoft.EntityFrameworkCore;
using RSVP.Application.Interfaces;
using RSVP.Infrastructure.Persistence;
using RSVP.Infrastructure.Repositories;
using MediatR;
using System.Reflection;
using RSVP.Application;
using RSVP.Application.Service;
using Microsoft.AspNetCore.Http;
using FluentValidation;
using RSVP.Application.Common.Behaviours;
using RSVP.Infrastructure.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
{
    
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<RsvpDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RsvpConnectionString")));



builder.Services.AddValidatorsFromAssembly(Assembly.Load("RSVP.Application"));



builder.Services.AddMediatR(cfg =>{

    cfg.RegisterServicesFromAssembly(Assembly.Load("RSVP.Application"));

    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    
 });

 var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Secret"];

// --- 2. Add Authentication Services ---
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
    };
});





builder.Services.AddScoped<IRsvpDbContext>(provider => provider.GetRequiredService<RsvpDbContext>());
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<IEventAccessService, EventAccessService>();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserReposistory, UserRepository>();
builder.Services.AddScoped<IRefreshReposistary, RefreshReposistary>();


builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
