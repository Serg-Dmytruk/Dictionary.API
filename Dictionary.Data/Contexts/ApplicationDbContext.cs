using Dictionary.Data.Configurations;
using Dictionary.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dictionary.Data.Contexts;

public class ApplicationDbContext : DbContext
{
    public virtual DbSet<Word> Words { get; set; } = null!;
    public virtual DbSet<Relation> Relations { get; set; } = null!;
    public virtual DbSet<PossibleTranslation> PossibleTranslations { get; set; } = null!;
    public virtual DbSet<Example> Examples { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        var envName = Environment.GetEnvironmentVariable("DICTIONARY_ENVIRONMENT");
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile($"appsettings.{envName}.json", false)
            .Build();

        var connection = config.GetConnectionString("DefaultConnection");
        builder.UseNpgsql(connection).UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new PossibleTranslationConfiguration());
        builder.ApplyConfiguration(new WordConfiguration());
        builder.ApplyConfiguration(new RelatedWordsConfiguration());
        builder.ApplyConfiguration(new ExampleConfiguration());
    }

}