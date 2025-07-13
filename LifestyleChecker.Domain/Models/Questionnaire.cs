namespace LifestyleChecker.Domain.Models
{
    /// <summary>
    /// Represents a questionnaire with a unique identifier and a name.
    /// </summary>
    /// <remarks>The <see cref="Questionnaire"/> class is used to create and manage questionnaires, each
    /// identified by a unique <see cref="Id"/>. The <see cref="Name"/> property is required and must be provided during
    /// instantiation.</remarks>
    public class Questionnaire : Auditable
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Gets or sets the name associated with the entity.
        /// This will be unique for each questionnaire, except for when an alternate version is created.
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Gets or sets the version number of the entity.
        /// </summary>
        public required int Version { get; set; }

        public ICollection<QuestionnaireQuestion> Questions { get; set; } = new List<QuestionnaireQuestion>();
        public ScoreEvaluationCriteria EvaluationCriteria { get; set; } = null!;
        public ICollection<QuestionnaireResponse> QuestionnaireResponses { get; set; } = new List<QuestionnaireResponse>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Questionnaire"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the questionnaire. Cannot be null or empty.</param>
        public Questionnaire()
        {
            Id = Guid.NewGuid();
        }
    }
}
