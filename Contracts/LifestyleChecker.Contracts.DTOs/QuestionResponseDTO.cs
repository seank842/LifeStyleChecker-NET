namespace LifestyleChecker.Contracts.DTOs
{
    public class QuestionResponseDTO
    {
        public required string Response { get; set; }
        public required Guid QuestionId { get; set; }
    }
}
