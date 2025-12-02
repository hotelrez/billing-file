using BillingFile.Application.Interfaces;
using BillingFile.Application.Mappings;
using BillingFile.Application.Services;
using BillingFile.Domain.Interfaces;
using BillingFile.Infrastructure.Data;
using BillingFile.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog (Console only - no file logging)
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Billing File API",
        Version = "v1",
        Description = "API for managing billing records"
    });
});

// Database Configuration - Multiple Databases
var memberPortalConnectionString = builder.Configuration.GetConnectionString("MemberPortalConnection");
var playConnectionString = builder.Configuration.GetConnectionString("PlayConnection");

builder.Services.AddDbContext<MemberPortalDbContext>(options =>
{
    options.UseSqlServer(memberPortalConnectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });
});

builder.Services.AddDbContext<PlayDbContext>(options =>
{
    options.UseSqlServer(playConnectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });
});

// Register repositories and services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Health Checks - Check both databases
builder.Services.AddHealthChecks()
    .AddDbContextCheck<MemberPortalDbContext>("MemberPortal Database")
    .AddDbContextCheck<PlayDbContext>("Play Database");

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Only redirect to HTTPS in production
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAll");

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

// Note: No database migrations needed - connecting to existing databases
Log.Information("Starting Billing File API");

app.Run();

