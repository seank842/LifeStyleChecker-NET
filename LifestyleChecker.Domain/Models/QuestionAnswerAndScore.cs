namespace LifestyleChecker.Domain.Models
{
    /// <summary>
    /// Represents an answer to a question along with its associated score and demographic information.
    /// </summary>
    /// <remarks>This class is used to store and manage the details of a specific answer to a question,
    /// including the score assigned to the answer and the age group it pertains to. It is typically used in scenarios
    /// where questions are evaluated and scored, such as surveys or assessments.</remarks>
    public class QuestionAnswerAndScore : Auditable
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Gets or sets the answer as a string.
        /// </summary>
        public required string Answer { get; set; }
        /// <summary>
        /// Gets or sets the score value.
        /// </summary>
        public required int Score { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the items are ordered.
        /// </summary>
        public required int Order { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier for the question.
        /// </summary>
        public required Guid QuestionId { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier for the age group.
        /// </summary>
        public required Guid AgeGroupId { get; set; }

        /// <summary>
        /// Gets or sets the questionnaire question associated with the current context.
        /// </summary>
        public QuestionnaireQuestion QuestionnaireQuestion { get; set; } = null!;
        /// <summary>
        /// Gets or sets the age group classification for the individual.
        /// </summary>
        public AgeGroup AgeGroup { get; set; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionAnswerAndScore"/> class with the specified question
        /// identifier, answer, and score.
        /// </summary>
        /// <param name="questionId">The unique identifier of the question. This value cannot be <see langword="null"/>.</param>
        /// <param name="answer">The answer provided for the question. This value cannot be <see langword="null"/> or empty.</param>
        /// <param name="score">The score associated with the answer. Must be a non-negative integer.</param>
        public QuestionAnswerAndScore()
        {
            Id = Guid.NewGuid();
        }
    }
}
