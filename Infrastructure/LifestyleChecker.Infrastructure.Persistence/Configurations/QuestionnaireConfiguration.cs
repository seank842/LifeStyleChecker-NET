using LifestyleChecker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LifestyleChecker.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the entity type for <see cref="Questionnaire"/>.
    /// </summary>
    /// <remarks>This configuration sets the primary key, required properties, and maximum length constraints
    /// for the <see cref="Questionnaire"/> entity. It ensures that the <c>Name</c> property is required and has a
    /// maximum length of 100 characters, and that the <c>CreatedAt</c> property is required.</remarks>
    public class QuestionnaireConfiguration : IEntityTypeConfiguration<Questionnaire>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Questionnaire> builder)
        {
            builder.HasKey(q => q.Id);
            builder.Property(q => q.Name).IsRequired().HasMaxLength(100);
            builder.Property(q => q.CreatedAt).IsRequired();
            builder.Property(q => q.Version).IsRequired();
            builder.HasMany(q => q.Questions)
                   .WithOne(qu => qu.Questionnaire)
                   .HasForeignKey(qu => qu.QuestionnaireId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(q => q.QuestionnaireResponses)
                   .WithOne(qr => qr.Questionnaire)
                   .HasForeignKey(qr => qr.QuestionnaireId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(q => q.EvaluationCriteria)
                   .WithOne(ec => ec.Questionnaire)
                   .HasForeignKey<ScoreEvaluationCriteria>(ec => ec.QuestionnaireId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
