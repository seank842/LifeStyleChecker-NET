using LifestyleChecker.SharedUtilities.Enums;

namespace LifestyleChecker.Domain.Models
{
    /// <summary>
    /// Represents a question within a questionnaire, including its text, answer type, and associated answers with
    /// scores.
    /// </summary>
    /// <remarks>This class is used to define a question in a questionnaire, specifying the text of the
    /// question, the type of answer expected, and a collection of possible answers with their scores. Each question is
    /// uniquely identified by a GUID.</remarks>
    public class QuestionnaireQuestion : Auditable
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Gets or sets the text of the question.
        /// </summary>
        public required string QuestionText { get; set; }
        /// <summary>
        /// Gets or sets the order index for items.
        /// </summary>
        public required uint Order { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the field is required.
        /// </summary>
        public required bool IsRequired { get; set; }
        /// <summary>
        /// Gets or sets the type of answer associated with the question.
        /// </summary>
        public required QuestionAnswerType AnswerType { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier for the questionnaire.
        /// </summary>
        public required Guid QuestionnaireId { get; set; }

        /// <summary>
        /// Gets or sets the questionnaire associated with the current context.
        /// </summary>
        public Questionnaire Questionnaire { get; set; } = null!;
        /// <summary>
        /// Gets or sets the collection of question-answer pairs along with their associated scores.
        /// </summary>
        public ICollection<QuestionAnswerAndScore> Answers { get; set; } = new List<QuestionAnswerAndScore>();
        public ICollection<QuestionResponse> Responses { get; set; } = new List<QuestionResponse>();

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionnaireQuestion"/> class with the specified question text
        /// and answer type.
        /// </summary>
        /// <remarks>The <see cref="Id"/> property is automatically generated as a new GUID for each
        /// instance of <see cref="QuestionnaireQuestion"/>.</remarks>
        /// <param name="questionText">The text of the question to be displayed in the questionnaire. Cannot be null or empty.</param>
        /// <param name="answerType">The type of answer expected for the question, represented by the <see cref="QuestionAnswerType"/>
        /// enumeration.</param>
        public QuestionnaireQuestion()
        {
            Id = Guid.NewGuid();
        }
    }
}
