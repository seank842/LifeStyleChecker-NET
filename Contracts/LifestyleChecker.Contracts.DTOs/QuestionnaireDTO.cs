namespace LifestyleChecker.Contracts.DTOs
{
    public class QuestionnaireDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public int Version { get; set; }
        public ICollection<QuestionDTO> Questions { get; set; } = new List<QuestionDTO>();
    }
}
