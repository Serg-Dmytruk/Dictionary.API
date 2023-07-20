using Dictionary.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dictionary.Data.Configurations;

public class RelatedWordsConfiguration : IEntityTypeConfiguration<Relation>
{
    public void Configure(EntityTypeBuilder<Relation> builder)
    {
        builder.HasKey(wr => new { wr.WordId, wr.RelatedWordId });
        
        builder.HasOne(wr => wr.Word)
            .WithMany(w => w.RelatedWords)
            .HasForeignKey(wr => wr.WordId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(wr => wr.RelatedWord)
            .WithMany(w => w.RelatedFromWords)
            .HasForeignKey(wr => wr.RelatedWordId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}