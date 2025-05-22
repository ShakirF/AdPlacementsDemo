using AdPlacements.Api.Filters;
using AdPlacements.Application.Interfaces;
using AdPlacements.Application.Services;
using AdPlacements.Infrastructure.Parsing;
using AdPlacements.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddNewtonsoftJson();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddSingleton<IPlatformRepository, InMemoryPlatformRepository>();
builder.Services.AddSingleton<PlatformFileParser>();
builder.Services.AddSingleton<IPlatformService, PlatformService>();

builder.Services.Configure<ApiBehaviorOptions>(o => o.SuppressModelStateInvalidFilter = true);


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
