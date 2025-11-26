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

builder.Services.AddScoped<IRsvpDbContext>(provider => provider.GetRequiredService<RsvpDbContext>());
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<IEventAccessService, EventAccessService>();



builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddControllers();
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
