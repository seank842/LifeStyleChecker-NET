using LifestyleChecker.Domain.Models;
using LifestyleChecker.SharedUtilities.Enums;

namespace LifestyleChecker.Infrastructure.Persistence.Seeding
{
    /// <summary>
    /// Provides methods to seed the database with initial data for age groups and a lifestyle questionnaire.
    /// </summary>
    /// <remarks>The <see cref="DataSeeder"/> class is responsible for populating the database with predefined
    /// age groups and a lifestyle questionnaire. The questionnaire includes questions about drinking, smoking, and
    /// exercise habits, with scores assigned based on the answers and age groups. It also sets evaluation criteria for
    /// the questionnaire.</remarks>
    public static class DataSeeder
    {
        public static async Task SeedAsync(LifestyleCheckerDbContext dbContext)
        {
            // Prevent duplicate seeding
            if (dbContext.AgeGroups.Any() || dbContext.Questionnaires.Any())
                return;

            var ageGroups = new List<AgeGroup>
            {
                new()
                {
                    Name = "16-21",
                    MinAge = 16,
                    MaxAge = 21,
                    Version = 1
                },
                new()
                {
                    Name = "22-40",
                    MinAge = 22,
                    MaxAge = 40,
                    Version = 1
                },
                new()
                {
                    Name = "41-65",
                    MinAge = 41,
                    MaxAge = 65,
                    Version = 1
                },
                new()
                {
                    Name = "66+",
                    MinAge = 66,
                    MaxAge = int.MaxValue,
                    Version = 1
                }
            };

            dbContext.AgeGroups.AddRange(ageGroups);
            await dbContext.SaveChangesAsync();

            // Reload age groups to ensure they have IDs from the database
            var ageGroupsFromDb = dbContext.AgeGroups.OrderBy(a => a.MinAge).ToList();

            var questionnaire = new Questionnaire
            {
                Name = "Lifestyle Questionnaire",
                Questions = new List<QuestionnaireQuestion>(),
                Version = 1
            };

            dbContext.Questionnaires.Add(questionnaire);
            await dbContext.SaveChangesAsync();

            var questions = new List<QuestionnaireQuestion>
            {
                new()
                {
                    QuestionText = "Do you drink on more than 2 days a week?",
                    AnswerType = QuestionAnswerType.YesNo,
                    QuestionnaireId = questionnaire.Id,
                    Order = 1,
                    IsRequired = true
                },
                new()
                {
                    QuestionText = "Do you smoke?",
                    AnswerType = QuestionAnswerType.YesNo,
                    QuestionnaireId = questionnaire.Id,
                    Order = 2,
                    IsRequired = true
                },
                new()
                {
                    QuestionText = "Do you excercise more than 2 days per week?",
                    AnswerType = QuestionAnswerType.YesNo,
                    QuestionnaireId = questionnaire.Id,
                    Order = 3,
                    IsRequired = true
                }
            };

            var questionAnswerAndScores = new List<QuestionAnswerAndScore>
            {
                new() { Answer = "Yes", Score = 1, AgeGroupId = ageGroupsFromDb[0].Id, QuestionId = questions[0].Id, Order = 2 },
                new() { Answer = "Yes", Score = 2, AgeGroupId = ageGroupsFromDb[1].Id, QuestionId = questions[0].Id, Order = 2 },
                new() { Answer = "Yes", Score = 3, AgeGroupId = ageGroupsFromDb[2].Id, QuestionId = questions[0].Id, Order = 2 },
                new() { Answer = "Yes", Score = 3, AgeGroupId = ageGroupsFromDb[3].Id, QuestionId = questions[0].Id, Order = 2 },
                new() { Answer = "No", Score = 0, AgeGroupId = ageGroupsFromDb[0].Id, QuestionId = questions[0].Id, Order = 1 },
                new() { Answer = "No", Score = 0, AgeGroupId = ageGroupsFromDb[1].Id, QuestionId = questions[0].Id, Order = 1 },
                new() { Answer = "No", Score = 0, AgeGroupId = ageGroupsFromDb[2].Id, QuestionId = questions[0].Id, Order = 1 },
                new() { Answer = "No", Score = 0, AgeGroupId = ageGroupsFromDb[3].Id, QuestionId = questions[0].Id, Order = 1 },
                new() { Answer = "Yes", Score = 2, AgeGroupId = ageGroupsFromDb[0].Id, QuestionId = questions[1].Id, Order = 2 },
                new() { Answer = "Yes", Score = 2, AgeGroupId = ageGroupsFromDb[1].Id, QuestionId = questions[1].Id, Order = 2 },
                new() { Answer = "Yes", Score = 2, AgeGroupId = ageGroupsFromDb[2].Id, QuestionId = questions[1].Id, Order = 2 },
                new() { Answer = "Yes", Score = 3, AgeGroupId = ageGroupsFromDb[3].Id, QuestionId = questions[1].Id, Order = 2 },
                new() { Answer = "No", Score = 0, AgeGroupId = ageGroupsFromDb[0].Id, QuestionId = questions[1].Id, Order = 1 },
                new() { Answer = "No", Score = 0, AgeGroupId = ageGroupsFromDb[1].Id, QuestionId = questions[1].Id, Order = 1 },
                new() { Answer = "No", Score = 0, AgeGroupId = ageGroupsFromDb[2].Id, QuestionId = questions[1].Id, Order = 1 },
                new() { Answer = "No", Score = 0, AgeGroupId = ageGroupsFromDb[3].Id, QuestionId = questions[1].Id, Order = 1 },
                new() { Answer = "Yes", Score = 0, AgeGroupId = ageGroupsFromDb[0].Id, QuestionId = questions[2].Id, Order = 1 },
                new() { Answer = "Yes", Score = 0, AgeGroupId = ageGroupsFromDb[1].Id, QuestionId = questions[2].Id, Order = 1 },
                new() { Answer = "Yes", Score = 0, AgeGroupId = ageGroupsFromDb[2].Id, QuestionId = questions[2].Id, Order = 1 },
                new() { Answer = "Yes", Score = 0, AgeGroupId = ageGroupsFromDb[3].Id, QuestionId = questions[2].Id, Order = 1 },
                new() { Answer = "No", Score = 1, AgeGroupId = ageGroupsFromDb[0].Id, QuestionId = questions[2].Id, Order = 2 },
                new() { Answer = "No", Score = 3, AgeGroupId = ageGroupsFromDb[1].Id, QuestionId = questions[2].Id, Order = 2 },
                new() { Answer = "No", Score = 2, AgeGroupId = ageGroupsFromDb[2].Id, QuestionId = questions[2].Id, Order = 2 },
                new() { Answer = "No", Score = 1, AgeGroupId = ageGroupsFromDb[3].Id, QuestionId = questions[2].Id, Order = 2 },
            };

            dbContext.QuestionnaireQuestions.AddRange(questions);
            await dbContext.SaveChangesAsync();

            dbContext.QuestionAnswerAndScores.AddRange(questionAnswerAndScores);
            await dbContext.SaveChangesAsync();

            // Set EvaluationCriteria after questionnaire is saved (so it has an Id)
            var evaluationCriteria = new ScoreEvaluationCriteria
            {
                CutOffScore = 3,
                ExceedsCriteriaMessage = "We think there are some simple things you could do to improve you quality of life, please phone to book an appointment",
                UnderOrEqualCriteriaMessage = "Thank you for answering our questions, we don't need to see you at this time. Keep up the good work!",
                QuestionnaireId = questionnaire.Id,
                OverCutOffMedicalAdvice = true
            };

            dbContext.ScoreEvaluationCriterias.Add(evaluationCriteria);
            await dbContext.SaveChangesAsync();
        }
    }
}
