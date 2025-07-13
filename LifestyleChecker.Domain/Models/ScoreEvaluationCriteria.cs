namespace LifestyleChecker.Domain.Models
{
    /// <summary>
    /// Represents the criteria used to evaluate scores, including associated age group and questionnaire details.
    /// </summary>
    /// <remarks>This class is used to define the parameters and messages for evaluating scores against a
    /// specified cutoff. It includes references to the related age group and questionnaire, and provides messages for
    /// scores that exceed or fall under the defined criteria.</remarks>
    public class ScoreEvaluationCriteria : Auditable
    {
        /// <summary>
        /// Gets or sets the unique identifier for the score evaluation criteria.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier for the questionnaire associated with the score evaluation criteria.
        /// </summary>
        public Guid QuestionnaireId { get; set; }
        /// <summary>
        /// Gets or sets the cutoff score used to determine the threshold for passing.
        /// </summary>
        public required int CutOffScore { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the medical advice is sought when the score exceeds the cutoff or is below the cutoff.
        /// </summary>
        /// <remarks><see cref="true"/>: When medical advice is needed if over cutoff, <see cref="false"/>: When medical advice is needed under or equal to the cutoff.</remarks>
        public required bool OverCutOffMedicalAdvice { get; set; }
        /// <summary>
        /// Gets or sets the resource key for the message displayed when criteria are exceeded.
        /// </summary>
        public required string ExceedsCriteriaMessage { get; set; }
        /// <summary>
        /// Gets or sets the resource key for the message displayed when criteria are not met.
        /// </summary>
        public required string UnderOrEqualCriteriaMessage { get; set; }

        /// <summary>
        /// Gets or sets the questionnaire associated with the current context.
        /// </summary>
        public Questionnaire Questionnaire { get; set; } = null!;
    }
}
