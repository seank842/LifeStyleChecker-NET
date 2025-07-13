namespace LifestyleChecker.Domain.Models
{
    /// <summary>
    /// Represents a response to a questionnaire, including the respondent's age group and their answers to individual
    /// questions.
    /// </summary>
    /// <remarks>This class is used to capture and store the details of a respondent's answers to a
    /// questionnaire.  It includes the respondent's age group at the time of response and a collection of individual
    /// question responses.</remarks>
    public class QuestionnaireResponse : Auditable
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Gets or sets the age group of the respondent at the time of response.
        /// </summary>
        public int AgeGroupAtTimeOfResponse { get; set; }
        /// <summary>
        /// Gets or sets the NHS number of the respondent.
        /// </summary>
        public string RespondentNHSNumber { get; set; }
        public required Guid QuestionnaireId { get; set; }

        /// <summary>
        /// Gets or sets the collection of responses to questions.
        /// </summary>
        public ICollection<QuestionResponse> QuestionResponses { get; set; } = new List<QuestionResponse>();
        public Questionnaire Questionnaire { get; set; }
    }
}
