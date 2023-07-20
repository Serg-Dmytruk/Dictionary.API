using Dictionary.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dictionary.Data.Configurations;

public class WordConfiguration : IEntityTypeConfiguration<Word>
{
    public void Configure(EntityTypeBuilder<Word> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.PossibleTranslations)
            .WithOne(x => x.Word).HasForeignKey(x => x.WordId);
    }
}