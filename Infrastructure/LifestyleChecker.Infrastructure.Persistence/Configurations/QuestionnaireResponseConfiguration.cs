using LifestyleChecker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LifestyleChecker.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the entity type for <see cref="QuestionnaireResponse"/>.
    /// </summary>
    /// <remarks>This configuration sets up the primary key, required properties, and relationships for the
    /// <see cref="QuestionnaireResponse"/> entity. It ensures that the <c>AgeGroupAtTimeOfResponse</c> and
    /// <c>CreatedAt</c> properties are required and establishes a one-to-many relationship with
    /// <c>QuestionResponses</c>.</remarks>
    public class QuestionnaireResponseConfiguration : IEntityTypeConfiguration<QuestionnaireResponse>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<QuestionnaireResponse> builder)
        {
            builder.HasKey(qr => qr.Id);
            builder.Property(qr => qr.AgeGroupAtTimeOfResponse).IsRequired();
            builder.Property(qr => qr.CreatedAt).IsRequired();
            builder.Property(qr => qr.RespondentNHSNumber)
                .IsRequired()
                .HasMaxLength(10);
            builder.HasMany(qr => qr.QuestionResponses)
                .WithOne(qr => qr.QuestionnaireResponse)
                .HasForeignKey(qr => qr.QuestionnaireResponseId);
            builder.Property(qr => qr.CreatedAt).IsRequired();
            builder.HasOne(qr => qr.Questionnaire)
                .WithMany(q => q.QuestionnaireResponses)
                .HasForeignKey(qr => qr.QuestionnaireId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
