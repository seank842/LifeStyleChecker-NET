using LifestyleChecker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LifestyleChecker.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the database schema for the <see cref="AgeGroup"/> entity type.
    /// </summary>
    /// <remarks>This configuration class defines the primary key, required properties, and unique index for
    /// the <see cref="AgeGroup"/> entity. It ensures that the <c>Name</c>, <c>MinAge</c>, <c>MaxAge</c>, and
    /// <c>Version</c> properties are required and sets a maximum length for the <c>Name</c> property. Additionally, it
    /// creates a unique index on the combination of <c>MinAge</c> and <c>MaxAge</c>.</remarks>
    public class AgeGroupConfiguration : IEntityTypeConfiguration<AgeGroup>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<AgeGroup> builder)
        {
            builder.HasKey(ag => ag.Id);
            builder.Property(ag => ag.Name).IsRequired().HasMaxLength(50);
            builder.Property(ag => ag.MinAge).IsRequired();
            builder.Property(ag => ag.MaxAge).IsRequired();
            builder.Property(ag => ag.Version).IsRequired();
            builder.Property(ag => ag.CreatedAt).IsRequired();
            builder.HasIndex(ag => new { ag.MinAge, ag.MaxAge }).IsUnique();
        }
    }
}
