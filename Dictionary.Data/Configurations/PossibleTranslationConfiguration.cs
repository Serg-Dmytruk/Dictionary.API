using Dictionary.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dictionary.Data.Configurations;

public class PossibleTranslationConfiguration : IEntityTypeConfiguration<PossibleTranslation>
{
    public void Configure(EntityTypeBuilder<PossibleTranslation> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Example);
        builder.HasIndex(x => x.Explanation);
        builder.HasIndex(x => x.Translation);
    }
}