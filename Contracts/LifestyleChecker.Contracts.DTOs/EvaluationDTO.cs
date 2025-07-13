using System.Text.Json.Serialization;

namespace LifestyleChecker.Contracts.DTOs
{
    /// <summary>
    /// Represents the result of an evaluation, including a recommendation for seeking medical advice and an associated
    /// message.
    /// </summary>
    public class EvaluationDTO
    {
        /// <summary>
        /// Gets or sets a value indicating whether medical advice should be sought.
        /// </summary>
        public bool SeekMedicalAdvice { get; set; }
        /// <summary>
        /// Gets or sets the message content.
        /// </summary>
        public string Message { get; set; } = null!;
        /// <summary>
        /// Gets or sets the total score accumulated by the player.
        /// </summary>
        public int TotalScore { get; set; }
        /// <summary>
        /// Gets or sets the date and time when the entity was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Gets or sets the name of the questionnaire.
        /// </summary>
        public QuestionnaireDTO Questionnaire { get; set; } = null!;
    }
}
