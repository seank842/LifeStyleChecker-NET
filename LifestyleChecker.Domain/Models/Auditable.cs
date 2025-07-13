namespace LifestyleChecker.Domain.Models
{
    /// <summary>
    /// Represents an entity that tracks creation timestamp.
    /// </summary>
    /// <remarks>The <see cref="Auditable"/> class provides properties to store the timestamps of when an
    /// entity was created. It is useful for maintaining audit trails in applications.</remarks>
    public class Auditable
    {
        /// <summary>
        /// Gets or sets the date and time when the entity was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Auditable"/> class, setting the creation and update timestamps
        /// to the current UTC time.
        /// </summary>
        /// <remarks>This constructor sets both the <c>CreatedAt</c> properties to
        /// the current UTC time, indicating the initial creation time for the instance.</remarks>
        public Auditable()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}
