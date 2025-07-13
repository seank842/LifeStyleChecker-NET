using LifestyleChecker.SharedUtilities.Enums;

namespace LifestyleChecker.Contracts.DTOs
{
    public class QuestionDTO
    {
        public Guid Id { get; set; }
        public required string QuestionText { get; set; }
        public required QuestionAnswerType AnswerType { get; set; }
        public required uint Order { get; set; }
        public required bool IsRequired { get; set; }
        public ICollection<QuestionAnswerDTO> AnswerOptions { get; set; } = new List<QuestionAnswerDTO>();
        public Guid QuestionnaireId { get; set; }
    }
}
