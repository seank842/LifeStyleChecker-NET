using System.ComponentModel.DataAnnotations;

namespace LifestyleChecker.Contracts.DTOs
{
    public class QuestionResponseDTO
    {
        [Required]
        public required string Response { get; set; }
        [Required]
        public required Guid QuestionId { get; set; }
    }
}
