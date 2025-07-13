using LifestyleChecker.Contracts.DTOs;
using LifestyleChecker.Domain.Models;
using LifestyleChecker.Service.Scoring;
using LifestyleChecker.SharedUtilities.Enums;
using Xunit.Gherkin.Quick;

namespace LifestyleChecker.Tests
{
    [FeatureFile("./Features/ScoringSystemEditedScores.feature")]
    public sealed class ScoringSystemEditedScoresTests : Feature
    {
        private int _age;
        private bool _drinksMoreThan2DaysAWeek;
        private bool _smoker;
        private bool _exerciseMoreThan1HourAWeek;
        private string _nhsNumber = "111111111";
        private EvaluationDTO _evaluationResult;

        [Given(@"I patient aged (\d+) starts the questionnaire")]
        public void GivenThePatientIsAged(int age)
        {
            _age = age;
        }

        [And(@"I answer the question 'Do you drink on more than 2 days a week' with (true|false)")]
        public void GivenIAnswerTheDrinkingQuestion(bool drinksMoreThan2DaysAWeek)
        {
            _drinksMoreThan2DaysAWeek = drinksMoreThan2DaysAWeek;
        }

        [And(@"I answer the question 'Do you smoke' with (true|false)")]
        public void GivenIAnswerTheSmokingQuestion(bool smoker)
        {
            _smoker = smoker;
        }
        [And(@"I answer the question 'Do you exercise more than 1 hour per week' with (true|false)")]
        public void GivenIAnswerTheExerciseQuestion(bool exerciseMoreThan1HourAWeek)
        {
            _exerciseMoreThan1HourAWeek = exerciseMoreThan1HourAWeek;
        }

        [When(@"I submit the questionnaire")]
        public void WhenISubmitTheQuestionnaire()
        {
            Questionnaire questionnaire = SetupQuestionnaire();
            var questionnaireResponse = new QuestionnaireResponse
            {
                Id = Guid.NewGuid(),
                QuestionnaireId = questionnaire.Id,
                AgeGroupAtTimeOfResponse = _age,
                RespondentNHSNumber = _nhsNumber,
                Questionnaire = questionnaire
            };

            questionnaireResponse.QuestionResponses = new List<QuestionResponse>
        {
            new()
            {
                Id = Guid.NewGuid(),
                QuestionId = questionnaire.Questions.First(q => q.QuestionText == "Do you drink on more than 2 days a week?").Id,
                Response = _drinksMoreThan2DaysAWeek ? "Yes" : "No",
                QuestionnaireResponse = questionnaireResponse,
                QuestionnaireResponseId = questionnaireResponse.Id,
                QuestionnaireQuestion = questionnaire.Questions.First(q => q.QuestionText == "Do you drink on more than 2 days a week?")
            },
            new()
            {
                Id = Guid.NewGuid(),
                QuestionId = questionnaire.Questions.First(q => q.QuestionText == "Do you smoke?").Id,
                Response = _smoker ? "Yes" : "No",
                QuestionnaireResponse = questionnaireResponse,
                QuestionnaireResponseId = questionnaireResponse.Id,
                QuestionnaireQuestion = questionnaire.Questions.First(q => q.QuestionText == "Do you smoke?")
            },
            new()
            {
                Id = Guid.NewGuid(),
                QuestionId = questionnaire.Questions.First(q => q.QuestionText == "Do you excercise more than 2 days per week?").Id,
                Response = _exerciseMoreThan1HourAWeek ? "Yes" : "No",
                QuestionnaireResponse = questionnaireResponse,
                QuestionnaireResponseId = questionnaireResponse.Id,
                QuestionnaireQuestion = questionnaire.Questions.First(q => q.QuestionText == "Do you excercise more than 2 days per week?")
            }
        };
            questionnaire.QuestionnaireResponses = new List<QuestionnaireResponse> { questionnaireResponse };

            var scoringService = new ScoringService();
            _evaluationResult = scoringService.EvaluateResults(questionnaire, questionnaireResponse);
        }

        [Then(@"I should see the risk score (\d+)")]
        public void ThenIShouldReceiveAScoreOf(int expectedScore)
        {
            Assert.Equal(expectedScore, _evaluationResult.TotalScore);
        }

        [And(@"display the message (.+)")]
        public void AndDisplayTheMessage(string expectedMessage)
        {
            expectedMessage = expectedMessage.Trim('"');
            Assert.Equal(expectedMessage, _evaluationResult.Message);
        }

        private static Questionnaire SetupQuestionnaire()
        {
            var ageGroups = new List<AgeGroup>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "16-21",
                MinAge = 16,
                MaxAge = 21,
                Version = 1
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "22-40",
                MinAge = 22,
                MaxAge = 40,
                Version = 1
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "41-65",
                MinAge = 41,
                MaxAge = 65,
                Version = 1
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "66+",
                MinAge = 66,
                MaxAge = int.MaxValue,
                Version = 1
            }
        };

            var questionnaire = new Questionnaire
            {
                Id = Guid.NewGuid(),
                Name = "Lifestyle Questionnaire",
                Questions = new List<QuestionnaireQuestion>(),
                Version = 1
            };

            var questions = new List<QuestionnaireQuestion>
        {
            new()
            {
                Id = Guid.NewGuid(),
                QuestionText = "Do you drink on more than 2 days a week?",
                AnswerType = QuestionAnswerType.YesNo,
                QuestionnaireId = questionnaire.Id,
                Order = 1,
                IsRequired = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                QuestionText = "Do you smoke?",
                AnswerType = QuestionAnswerType.YesNo,
                QuestionnaireId = questionnaire.Id,
                Order = 2,
                IsRequired = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                QuestionText = "Do you excercise more than 2 days per week?",
                AnswerType = QuestionAnswerType.YesNo,
                QuestionnaireId = questionnaire.Id,
                Order = 3,
                IsRequired = true
            }
        };

            var questionAnswerAndScores = new List<QuestionAnswerAndScore>
        {
            new() { Answer = "Yes", Score = 2, AgeGroupId = ageGroups[0].Id, AgeGroup = ageGroups[0], QuestionId = questions[0].Id, QuestionnaireQuestion = questions[0], Order = 2 },
            new() { Answer = "Yes", Score = 2, AgeGroupId = ageGroups[1].Id, AgeGroup = ageGroups[1], QuestionId = questions[0].Id, QuestionnaireQuestion = questions[0], Order = 2 },
            new() { Answer = "Yes", Score = 1, AgeGroupId = ageGroups[2].Id, AgeGroup = ageGroups[2], QuestionId = questions[0].Id, QuestionnaireQuestion = questions[0], Order = 2 },
            new() { Answer = "Yes", Score = 3, AgeGroupId = ageGroups[3].Id, AgeGroup = ageGroups[3], QuestionId = questions[0].Id, QuestionnaireQuestion = questions[0], Order = 2 },
            new() { Answer = "No", Score = 0, AgeGroupId = ageGroups[0].Id, AgeGroup = ageGroups[0], QuestionId = questions[0].Id, QuestionnaireQuestion = questions[0], Order = 1 },
            new() { Answer = "No", Score = 0, AgeGroupId = ageGroups[1].Id, AgeGroup = ageGroups[1], QuestionId = questions[0].Id, QuestionnaireQuestion = questions[0], Order = 1 },
            new() { Answer = "No", Score = 0, AgeGroupId = ageGroups[2].Id, AgeGroup = ageGroups[2], QuestionId = questions[0].Id, QuestionnaireQuestion = questions[0], Order = 1 },
            new() { Answer = "No", Score = 0, AgeGroupId = ageGroups[3].Id, AgeGroup = ageGroups[3], QuestionId = questions[0].Id, QuestionnaireQuestion = questions[0], Order = 1 },
            new() { Answer = "Yes", Score = 1, AgeGroupId = ageGroups[0].Id, AgeGroup = ageGroups[0], QuestionId = questions[1].Id, QuestionnaireQuestion = questions[1], Order = 2 },
            new() { Answer = "Yes", Score = 1, AgeGroupId = ageGroups[1].Id, AgeGroup = ageGroups[1], QuestionId = questions[1].Id, QuestionnaireQuestion = questions[1], Order = 2 },
            new() { Answer = "Yes", Score = 3, AgeGroupId = ageGroups[2].Id, AgeGroup = ageGroups[2], QuestionId = questions[1].Id, QuestionnaireQuestion = questions[1], Order = 2 },
            new() { Answer = "Yes", Score = 3, AgeGroupId = ageGroups[3].Id, AgeGroup = ageGroups[3], QuestionId = questions[1].Id, QuestionnaireQuestion = questions[1], Order = 2 },
            new() { Answer = "No", Score = 0, AgeGroupId = ageGroups[0].Id, AgeGroup = ageGroups[0], QuestionId = questions[1].Id, QuestionnaireQuestion = questions[1], Order = 1 },
            new() { Answer = "No", Score = 0, AgeGroupId = ageGroups[1].Id, AgeGroup = ageGroups[1], QuestionId = questions[1].Id, QuestionnaireQuestion = questions[1], Order = 1 },
            new() { Answer = "No", Score = 0, AgeGroupId = ageGroups[2].Id, AgeGroup = ageGroups[2], QuestionId = questions[1].Id, QuestionnaireQuestion = questions[1], Order = 1 },
            new() { Answer = "No", Score = 0, AgeGroupId = ageGroups[3].Id, AgeGroup = ageGroups[3], QuestionId = questions[1].Id, QuestionnaireQuestion = questions[1], Order = 1 },
            new() { Answer = "Yes", Score = 0, AgeGroupId = ageGroups[0].Id, AgeGroup = ageGroups[0], QuestionId = questions[2].Id, QuestionnaireQuestion = questions[2], Order = 1 },
            new() { Answer = "Yes", Score = 0, AgeGroupId = ageGroups[1].Id, AgeGroup = ageGroups[1], QuestionId = questions[2].Id, QuestionnaireQuestion = questions[2], Order = 1 },
            new() { Answer = "Yes", Score = 0, AgeGroupId = ageGroups[2].Id, AgeGroup = ageGroups[2], QuestionId = questions[2].Id, QuestionnaireQuestion = questions[2], Order = 1 },
            new() { Answer = "Yes", Score = 0, AgeGroupId = ageGroups[3].Id, AgeGroup = ageGroups[3], QuestionId = questions[2].Id, QuestionnaireQuestion = questions[2], Order = 1 },
            new() { Answer = "No", Score = 2, AgeGroupId = ageGroups[0].Id, AgeGroup = ageGroups[0], QuestionId = questions[2].Id, QuestionnaireQuestion = questions[2], Order = 2 },
            new() { Answer = "No", Score = 1, AgeGroupId = ageGroups[1].Id, AgeGroup = ageGroups[1], QuestionId = questions[2].Id, QuestionnaireQuestion = questions[2], Order = 2 },
            new() { Answer = "No", Score = 2, AgeGroupId = ageGroups[2].Id, AgeGroup = ageGroups[2], QuestionId = questions[2].Id, QuestionnaireQuestion = questions[2], Order = 2 },
            new() { Answer = "No", Score = 3, AgeGroupId = ageGroups[3].Id, AgeGroup = ageGroups[3], QuestionId = questions[2].Id, QuestionnaireQuestion = questions[2], Order = 2 },
        };

            var evaluationCriteria = new ScoreEvaluationCriteria
            {
                Id = Guid.NewGuid(),
                CutOffScore = 3,
                ExceedsCriteriaMessage = "We think there are some simple things you could do to improve you quality of life, please phone to book an appointment",
                UnderOrEqualCriteriaMessage = "Thank you for answering our questions, we don't need to see you at this time. Keep up the good work!",
                QuestionnaireId = questionnaire.Id,
                Questionnaire = questionnaire,
                OverCutOffMedicalAdvice = true
            };

            questionnaire.EvaluationCriteria = evaluationCriteria;
            questionnaire.Questions = questions;
            // Replace the line using ForEach with a foreach loop
            foreach (var q in questionnaire.Questions)
            {
                q.Answers = questionAnswerAndScores.Where(a => a.QuestionId == q.Id).ToList();
                q.Questionnaire = questionnaire;
            }

            return questionnaire;
        }
    }
}
