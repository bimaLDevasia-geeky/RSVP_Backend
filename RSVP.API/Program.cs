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
using RSVP.API.MIddleware;
using Hangfire;
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
var secretKey = jwtSettings["SecretKey"];

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

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));



builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "AllowAngularOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});


builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("RsvpConnectionString"), new Hangfire.SqlServer.SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

builder.Services.AddHangfireServer();



builder.Services.AddScoped<IRsvpDbContext>(provider => provider.GetRequiredService<RsvpDbContext>());
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<IEventAccessService, EventAccessService>();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserReposistory, UserRepository>();
builder.Services.AddScoped<IRefreshReposistary, RefreshReposistary>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IAttendieRepository, AttendieRepository>();
builder.Services.AddScoped<IPhotoService, PhotoService>();

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
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

app.UseHangfireDashboard("/hangfire");
  
RecurringJob.AddOrUpdate<SentNotification>(
    "SendEventReminderNotifications",
    job => job.SentNotificationToUser(),
     "*/10 * * * * *" );

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("AllowAngularOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
