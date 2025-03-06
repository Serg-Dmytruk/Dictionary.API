using System.Text;
using Dictionary.Application;
using Dictionary.Application.Options;
using Dictionary.Application.Services.ParseServices;
using Dictionary.Data;
using Serilog;
using Serilog.Events;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
Environment.SetEnvironmentVariable("DICTIONARY_ENVIRONMENT", environment);
var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().WriteTo.Console(LogEventLevel.Warning)
    .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "log.txt"),
        LogEventLevel.Warning, rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Logging.AddSerilog(logger);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDatabase(builder.Configuration.GetConnectionString("DefaultConnection")!);
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddHostedService<ParserService>();

builder.Services.Configure<ParserOptions>(builder.Configuration.GetSection("ParserOptions"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Console.OutputEncoding = Encoding.UTF8;
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await using var scope = app.Services.CreateAsyncScope();
var dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
await dbInitializer.InitializeAsync();

app.Run();