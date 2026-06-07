using Asp.Versioning;
using Hangfire;
using Hangfire.SqlServer;
using Insurance.API.Middleware;
using Insurance.Application.Interfaces;
using Insurance.Application.Mappings;
using Insurance.Infrastructure.Data;
using Insurance.Infrastructure.Repository;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Insurance.Application.Interfaces;
using Insurance.Infrastructure.Repositories;
using Serilog;
using System.Threading.RateLimiting;
using Asp.Versioning;
using Insurance.Application.Interfaces;
using Insurance.Infrastructure.Repositories;
using Insurance.Application.Interfaces;
using Insurance.Infrastructure.Repository;
using Hangfire;
using Hangfire.SqlServer;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/log-.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

#region AutoMapper

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

#endregion


// builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});


builder.Services.AddScoped<IQuoteRepo, QuoteRepo>();

builder.Services.AddScoped<IPremiumScheduleRepo, PremiumScheduleRepo>();

#region Database

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

#endregion

#region Controllers

builder.Services.AddControllers();

#endregion

#region Swagger

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

#region API Versioning

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

#endregion

#region Memory Cache

builder.Services.AddMemoryCache();

#endregion

#region Rate Limiting

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

#endregion

#region Repositories



builder.Services.AddScoped<IPolicyRepo, PolicyRepo>();

#endregion

#region Hangfire

builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer();

#endregion

builder.Services.AddMemoryCache();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IProductRepository,
    ProductRepository>();

builder.Services.AddScoped<IProductBenefitRepository,
    ProductBenefitRepository>();

builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer();


var app = builder.Build();

//#region Hangfire Dashboard & Jobs

//app.UseHangfireDashboard();

//RecurringJob.AddOrUpdate<IPremiumScheduleRepo>(
//    "PremiumReminderJob",
//    x => x.SendRemindersAsync(),
//    Cron.Minutely);

//#endregion
app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<IPremiumScheduleRepo>(
    "PremiumReminderJob",
    x => x.SendRemindersAsync(),
    Cron.Minutely);



Log.Information("Insurance API Started Successfully");

#region Middleware Pipeline

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

#endregion

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