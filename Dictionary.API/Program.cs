using System.Text;
using Dictionary.Application.Options;
using Dictionary.Application.Services.ParseServices;
using Dictionary.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
Environment.SetEnvironmentVariable("DICTIONARY_ENVIRONMENT", environment);
var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().WriteTo.Console(LogEventLevel.Warning)
    .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "log.txt"),
        LogEventLevel.Warning, rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Add services to the container.
builder.Logging.AddSerilog(logger);

var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(defaultConnection).UseSnakeCaseNamingConvention());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ParserOptions>(builder.Configuration.GetSection("ParserOptions"));
builder.Services.AddHostedService<ParserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Console.OutputEncoding = Encoding.UTF8;
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();