using LifestyleChecker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LifestyleChecker.Infrastructure.Persistence.Configurations
{
    public class QuestionResponseConfiguration : IEntityTypeConfiguration<QuestionResponse>
    {
        /// <summary>
        /// Configures the <see cref="QuestionResponse"/> entity type.
        /// </summary>
        /// <remarks>This method sets up the primary key, required properties, and relationships for the
        /// <see cref="QuestionResponse"/> entity. It ensures that the <c>Response</c>, <c>QuestionId</c>, and
        /// <c>QuestionnaireResponseId</c> properties are required, and configures the relationships with <see
        /// cref="QuestionnaireQuestion"/> and <see cref="QuestionnaireResponse"/> entities.</remarks>
        /// <param name="builder">The builder used to configure the <see cref="QuestionResponse"/> entity type.</param>
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<QuestionResponse> builder)
        {
            builder.HasKey(qr => qr.Id);
            builder.Property(qr => qr.Response).IsRequired();
            builder.Property(qr => qr.QuestionId).IsRequired();
            builder.Property(qr => qr.QuestionnaireResponseId).IsRequired();
            builder.Property(qr => qr.CreatedAt).IsRequired();
            builder.HasOne(qr => qr.QuestionnaireQuestion)
                .WithMany(q => q.Responses)
                .HasForeignKey(qr => qr.QuestionId);
            builder.HasOne(qr => qr.QuestionnaireResponse)
                .WithMany(qr => qr.QuestionResponses)
                .HasForeignKey(qr => qr.QuestionnaireResponseId);
        }
    }
}
