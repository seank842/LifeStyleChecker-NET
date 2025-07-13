using LifestyleChecker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LifestyleChecker.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the database schema for the <see cref="ScoreEvaluationCriteria"/> entity type.
    /// </summary>
    /// <remarks>This configuration class specifies the primary key, required properties, and relationships
    /// for the <see cref="ScoreEvaluationCriteria"/> entity. It ensures that the <c>CutOffScore</c>,
    /// <c>ExceedsCriteriaMessageResx</c>, and <c>UnderCriteriaMessageResx</c> properties are required and sets a
    /// maximum length of 100 characters for the message properties. Additionally, it defines foreign key relationships
    /// with the <c>AgeGroup</c> and <c>Questionnaire</c> entities, using <c>Restrict</c> delete behavior to prevent
    /// cascading deletions.</remarks>
    public class ScoreEvaluationCriteriaConfiguration : IEntityTypeConfiguration<ScoreEvaluationCriteria>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ScoreEvaluationCriteria> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CutOffScore)
                .IsRequired();
            builder.Property(x => x.OverCutOffMedicalAdvice)
                .IsRequired();
            builder.Property(x => x.ExceedsCriteriaMessage)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(x => x.UnderOrEqualCriteriaMessage)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasOne(x => x.Questionnaire)
                .WithOne(x => x.EvaluationCriteria)
                .HasForeignKey<ScoreEvaluationCriteria>(x => x.QuestionnaireId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
