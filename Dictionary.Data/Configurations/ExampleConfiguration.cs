using Dictionary.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dictionary.Data.Configurations;

public class ExampleConfiguration : IEntityTypeConfiguration<Example>
{
    public void Configure(EntityTypeBuilder<Example> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Value);
        builder.HasOne(x => x.PossibleTranslation)
            .WithMany(x => x.Examples).HasForeignKey(x => x.PossibleTranslationId);
    }
}