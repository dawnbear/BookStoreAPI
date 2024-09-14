using BookStoreAPI.Data;
using BookStoreAPI.Models;
using BookStoreAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry;
using System.Text;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using BookStoreAPI.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

var serviceName = builder.Environment.ApplicationName;
var serviceVersion = "1.0.0";

// config OpenTelemetry resource
var resourceBuilder = ResourceBuilder.CreateDefault()
   .AddService(serviceName: serviceName, serviceVersion: serviceVersion);

// set trace
using var tracerProvider = Sdk.CreateTracerProviderBuilder()
   .SetResourceBuilder(resourceBuilder)
   .AddSource("BookStoreAPI")
   .AddConsoleExporter()
   .Build();

// set metric
using var meterProvider = Sdk.CreateMeterProviderBuilder()
   .SetResourceBuilder(resourceBuilder)
   .AddMeter("BookStoreAPI")
   .AddConsoleExporter()
   .Build();

// set logging
using var loggerFactory = LoggerFactory.Create(
    builder => builder.AddOpenTelemetry(
        options =>
        {
            options.AddConsoleExporter();
            options.SetResourceBuilder(resourceBuilder);
        }));


builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

// Add services to the container.
builder.Services.AddControllers();

// Add InMemory Database
builder.Services.AddDbContext<BookstoreDbContext>(option => option.UseInMemoryDatabase("BookstoreDb"));

// Add Identity service
builder.Services.AddIdentityCore<BookstoreUser>().AddEntityFrameworkStores<BookstoreDbContext>();

builder.Services.AddScoped<JwtTokenService>();

var jwtSetting = builder.Configuration.GetSection("JwtSetting").Get<JwtSetting>();

// Add Jwt configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSetting.Issuer,
            ValidAudience = jwtSetting.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key))
        };
    });
builder.Services.AddAuthorization();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme{
                Reference =new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id ="Bearer"
                }
            },new string[]{ }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();