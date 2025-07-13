using LifestyleChecker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LifestyleChecker.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the entity of type <see cref="QuestionnaireQuestion"/>.
    /// </summary>
    /// <remarks>This configuration sets up the primary key, required properties, and relationships for the
    /// <see cref="QuestionnaireQuestion"/> entity. It ensures that the <c>QuestionText</c> and <c>AnswerType</c>
    /// properties are required and limits the <c>QuestionText</c> to a maximum length of 500 characters. The
    /// configuration also establishes relationships with the <c>Answers</c>, <c>Responses</c>, and <c>Questionnaire</c>
    /// entities, enforcing referential integrity with <c>Restrict</c> delete behavior.</remarks>
    public class QuestionnaireQuestionConfiguration : IEntityTypeConfiguration<QuestionnaireQuestion>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<QuestionnaireQuestion> builder)
        {
            builder.HasKey(q => q.Id);
            builder.Property(q => q.QuestionText).IsRequired().HasMaxLength(500);
            builder.Property(q => q.AnswerType).IsRequired();
            builder.Property(q => q.CreatedAt).IsRequired();
            builder.Property(q => q.Order).IsRequired();
            builder.HasOne(q => q.Questionnaire)
                   .WithMany(q => q.Questions)
                   .HasForeignKey(q => q.QuestionnaireId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(q => q.Answers)
                   .WithOne(a => a.QuestionnaireQuestion)
                   .HasForeignKey(a => a.QuestionId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(q => q.Responses)
                   .WithOne(r => r.QuestionnaireQuestion)
                   .HasForeignKey(r => r.QuestionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
