using LifestyleChecker.Contracts.DTOs;
using LifestyleChecker.Domain.Models;

namespace LifestyleChecker.Service.Scoring
{
    public interface IScoringService
    {
        EvaluationDTO EvaluateResults(Questionnaire questionnaire, QuestionnaireResponse questionnaireResponse);
    }
}
