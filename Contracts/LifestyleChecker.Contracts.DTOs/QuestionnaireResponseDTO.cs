using System.ComponentModel.DataAnnotations;

namespace LifestyleChecker.Contracts.DTOs
{
    public class QuestionnaireResponseDTO
    {
        [Required]
        public required Guid QuestionnaireId { get; set; }
        public ICollection<QuestionResponseDTO> QuestionResponses { get; set; } = new List<QuestionResponseDTO>();
    }
}
