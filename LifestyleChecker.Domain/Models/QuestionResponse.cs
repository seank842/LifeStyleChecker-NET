namespace LifestyleChecker.Domain.Models
{
    /// <summary>
    /// Represents a response to a specific question within a questionnaire.
    /// </summary>
    /// <remarks>This class is used to store the response details for a question, including the unique
    /// identifiers for the question and the associated questionnaire response. It inherits from the <see
    /// cref="Auditable"/> class, which provides auditing capabilities.</remarks>
    public class QuestionResponse : Auditable
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Gets or sets the response message.
        /// </summary>
        public required string Response { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier for the question.
        /// </summary>
        public required Guid QuestionId { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier for the questionnaire response.
        /// </summary>
        public required Guid QuestionnaireResponseId { get; set; }

        /// <summary>
        /// Gets or sets the questionnaire question associated with the current context.
        /// </summary>
        public QuestionnaireQuestion QuestionnaireQuestion { get; set; } = null!;
        /// <summary>
        /// Gets or sets the response to the questionnaire.
        /// </summary>
        public QuestionnaireResponse QuestionnaireResponse { get; set; } = null!;
    }
}
