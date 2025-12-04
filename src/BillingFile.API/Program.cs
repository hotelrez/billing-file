using BillingFile.API.Authentication;
using BillingFile.Application.Interfaces;
using BillingFile.Application.Mappings;
using BillingFile.Application.Services;
using BillingFile.Domain.Entities;
using BillingFile.Domain.Interfaces;
using BillingFile.Infrastructure.Data;
using BillingFile.Infrastructure.Repositories;
using BillingFile.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog (Console only - no file logging)
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Add Basic Authentication
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Services.AddAuthorization();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Billing File API",
        Version = "v1",
        Description = "API for managing billing records - Protected with Basic Authentication"
    });
    
    // Add Basic Authentication to Swagger
    options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authentication header using the Bearer scheme."
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            Array.Empty<string>()
        }
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
builder.Services.AddScoped<IBillingDataAccess, BillingDataAccess>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

// Register CSV import services
builder.Services.AddScoped<ICsvImportService<HotelBillingCurrency>, HotelCurrencyCsvImportService>();

// Register Currency Conversion Service with HttpClient
builder.Services.AddHttpClient<ICurrencyConversionService, CurrencyConversionService>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

// Note: No database migrations needed - connecting to existing databases
Log.Information("Starting Billing File API");

app.Run();

