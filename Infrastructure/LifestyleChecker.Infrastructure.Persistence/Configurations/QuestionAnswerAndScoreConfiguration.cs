using LifestyleChecker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LifestyleChecker.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Configures the entity of type <see cref="QuestionAnswerAndScore"/>.
    /// </summary>
    /// <remarks>This configuration sets up the primary key, required properties, and relationships for the
    /// <see cref="QuestionAnswerAndScore"/> entity. It ensures that the <c>QuestionId</c>, <c>Answer</c>, and
    /// <c>Score</c> properties are required. Additionally, it establishes foreign key relationships with the <see
    /// cref="QuestionnaireQuestion"/> and <see cref="AgeGroup"/> entities.</remarks>
    public class QuestionAnswerAndScoreConfiguration : IEntityTypeConfiguration<QuestionAnswerAndScore>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<QuestionAnswerAndScore> builder)
        {
            builder.HasKey(qas => qas.Id);
            builder.Property(qas => qas.Answer).IsRequired();
            builder.Property(qas => qas.Score).IsRequired();
            builder.Property(qas => qas.QuestionId).IsRequired();
            builder.Property(qas => qas.AgeGroupId).IsRequired();
            builder.Property(qas => qas.CreatedAt).IsRequired();

            builder.HasOne(qas => qas.QuestionnaireQuestion)
                .WithMany(q => q.Answers)
                .HasForeignKey(qas => qas.QuestionId);
            builder.HasOne(qas=>qas.AgeGroup)
                .WithMany(aG=> aG.QuestionAnswerAndScores)
                .HasForeignKey(qas=>qas.AgeGroupId);
        }
    }
}
