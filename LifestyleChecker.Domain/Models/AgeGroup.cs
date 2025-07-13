namespace LifestyleChecker.Domain.Models
{
    /// <summary>
    /// Represents a categorization of individuals based on age, identified by a unique identifier and a name.
    /// </summary>
    public class AgeGroup : Auditable
    {
        /// <summary>
        /// Gets or sets the unique identifier for the age group.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Gets or sets the name of the age group.
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Gets or sets the minimum age required for eligibility.
        /// </summary>
        public required int MinAge { get; set; }
        /// <summary>
        /// Gets or sets the maximum allowable age in years.
        /// </summary>
        public required int MaxAge { get; set; }
        /// <summary>
        /// Gets or sets the version number of the current instance.
        /// </summary>
        public required int Version { get; set; }

        /// <summary>
        /// Gets or sets the collection of question-answer pairs along with their associated scores.
        /// </summary>
        public ICollection<QuestionAnswerAndScore> QuestionAnswerAndScores { get; set; } = new List<QuestionAnswerAndScore>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AgeGroup"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the age group. Cannot be null or empty.</param>
        public AgeGroup()
        {
            Id = Guid.NewGuid();
        }
    }
}
