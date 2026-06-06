using Asp.Versioning;
using Insurance.API.Middleware;
using Insurance.Application.Interface;
using Insurance.Application.Mappings;
using Insurance.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
using Serilog;
using System.Threading.RateLimiting;
using Insurance.Infrastructure.Repository;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/log-.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});
//builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddMemoryCache();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(
        "ApiThrottle",
        opt =>
        {
            opt.PermitLimit = 5;
            opt.Window = TimeSpan.FromMinutes(1);
            opt.QueueLimit = 0;
        });
});

/* SERVICES */

builder.Services.AddScoped<IPolicyRepo, PolicyRepo>();



var app = builder.Build();

Log.Information("Insurance API Started Successfully");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

try
{
    Log.Information("Application Starting");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application Failed To Start");
}
finally
{
    Log.CloseAndFlush();
}