using LifestyleChecker.Domain.Models;
using LifestyleChecker.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace LifestyleChecker.Infrastructure.Persistence
{
    /// <summary>
    /// Represents the database context for the Lifestyle Checker application, providing access to the application's
    /// data models.
    /// </summary>
    /// <remarks>This context is used to interact with the database tables related to age groups,
    /// questionnaires, and responses. It configures the entity models using specific configurations during the model
    /// creation process.</remarks>
    public class LifestyleCheckerDbContext(DbContextOptions<LifestyleCheckerDbContext> options) : DbContext(options)
    {
        public DbSet<AgeGroup> AgeGroups { get; set; }
        public DbSet<QuestionAnswerAndScore> QuestionAnswerAndScores { get; set; }
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<QuestionnaireQuestion> QuestionnaireQuestions { get; set; }
        public DbSet<QuestionnaireResponse> QuestionnaireResponses { get; set; }
        public DbSet<QuestionResponse> QuestionResponses { get; set; }
        public DbSet<ScoreEvaluationCriteria> ScoreEvaluationCriterias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AgeGroupConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionAnswerAndScoreConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionnaireConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionnaireQuestionConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionnaireResponseConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionResponseConfiguration());
            modelBuilder.ApplyConfiguration(new ScoreEvaluationCriteriaConfiguration());

            base.OnModelCreating(modelBuilder);
        }


    }
}
