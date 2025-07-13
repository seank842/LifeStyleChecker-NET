using LifestyleChecker.Contracts.DTOs;
using LifestyleChecker.Domain.Models;

namespace LifestyleChecker.Service.Scoring
{
    /// <summary>
    /// Provides functionality to evaluate questionnaire responses against predefined criteria.
    /// </summary>
    /// <remarks>The <see cref="ScoringService"/> class is designed to process responses to a questionnaire
    /// and determine if the total score meets or exceeds the specified evaluation criteria. It ensures that all
    /// questions have corresponding responses and that each response is valid based on the age group
    /// criteria.</remarks>
    public class ScoringService : IScoringService
    {
        /// <summary>
        /// Evaluates the responses to a questionnaire and calculates the total score based on predefined criteria.
        /// </summary>
        /// <param name="questionnaire">The questionnaire containing the questions and evaluation criteria. Cannot be null.</param>
        /// <param name="questionnaireResponse">The responses to the questionnaire. Must contain at least one question response and cannot be null.</param>
        /// <returns>An <see cref="EvaluationDTO"/> containing the total score, a message based on the evaluation criteria, and
        /// an indication of whether medical advice is needed.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="questionnaire"/> or <paramref name="questionnaireResponse"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the questionnaire's evaluation criteria are not defined, if the questionnaire response contains no
        /// question responses, if the number of required questions in the questionnaire is not equal to or greater than
        /// the number of responses provided, if any question in the questionnaire lacks answers, or if any answer lacks
        /// a valid age group.</exception>
        public EvaluationDTO EvaluateResults(Questionnaire questionnaire, QuestionnaireResponse questionnaireResponse)
        {
            #region Parameter Validation
            if (questionnaire == null)
            {
                throw new ArgumentNullException(nameof(questionnaire), "Questionnaire cannot be null.");
            }
            if (questionnaireResponse == null)
            {
                throw new ArgumentNullException(nameof(questionnaireResponse), "QuestionnaireResponse cannot be null.");
            }
            if (questionnaire.EvaluationCriteria == null)
            {
                throw new ArgumentException("Questionnaire evaluation criteria not defined.");
            }
            if (questionnaireResponse.QuestionResponses.Count == 0)
            {
                throw new ArgumentException("QuestionnaireResponse must contain at least one question response.");
            }
            if (questionnaire.Questions.Where(q=>q.IsRequired).Count() < questionnaireResponse.QuestionResponses.Count)
            {
                throw new ArgumentException("The number of required questions in the questionnaire is not equal to or greater than the number of responses provided.");
            }
            if (!questionnaire.Questions.All(q => q.Answers.All(a => a.AgeGroup != null)))
            {
                throw new ArgumentException("All answers must have a valid age group.");
            }
            #endregion

            int totalScore = 0;

            questionnaire.Questions
                .ToList()
                .ForEach(q =>
                {
                    var response = questionnaireResponse.QuestionResponses.FirstOrDefault(r => r.QuestionId == q.Id) ?? throw new ArgumentException($"No response found for question {q.Id}.");
                    var answer = q.Answers.FirstOrDefault(a => a.Answer == response.Response
                                                               && (questionnaireResponse.AgeGroupAtTimeOfResponse >= a.AgeGroup.MinAge
                                                                   && questionnaireResponse.AgeGroupAtTimeOfResponse <= a.AgeGroup.MaxAge)) ?? throw new ArgumentException($"No valid answer found for question {q.Id} with answer {response.Response}.");
                    totalScore += answer.Score;
                });
#pragma warning disable CS8602 // Dereference of a possibly null reference. Null check is done above.
            return new EvaluationDTO
            {
                // utilise bitwise flipped-XOR to determine if medical advice is needed
                SeekMedicalAdvice = !(totalScore > questionnaire.EvaluationCriteria.CutOffScore ^ questionnaire.EvaluationCriteria.OverCutOffMedicalAdvice),
                Message = totalScore > questionnaire.EvaluationCriteria.CutOffScore
                    ? questionnaire.EvaluationCriteria.ExceedsCriteriaMessage
                    : questionnaire.EvaluationCriteria.UnderOrEqualCriteriaMessage,
                TotalScore = totalScore,
                CreatedAt = questionnaireResponse.CreatedAt,
                Questionnaire = new QuestionnaireDTO
                {
                    Id = questionnaire.Id,
                    Name = questionnaire.Name,
                    Version = questionnaire.Version
                }
            };
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}
