using LifestyleChecker.Contracts.DTOs;
using LifestyleChecker.Domain.Models;

namespace LifestyleChecker.ApiService.Mapping
{
    /// <summary>
    /// Provides extension methods for mapping between <see cref="QuestionResponse"/> entities and <see
    /// cref="QuestionResponseDTO"/> data transfer objects.
    /// </summary>
    /// <remarks>This static class includes methods to convert between the domain model and its corresponding
    /// DTO representation, facilitating data transfer between different layers of the application.</remarks>
    public static class QuestionResponseMapping
    {
        /// <summary>
        /// Converts a <see cref="QuestionResponse"/> object to a <see cref="QuestionResponseDTO"/> object.
        /// </summary>
        /// <param name="answer">The <see cref="QuestionResponse"/> instance to convert. Cannot be null.</param>
        /// <returns>A <see cref="QuestionResponseDTO"/> object containing the data from the specified <paramref name="answer"/>.
        /// Returns null if <paramref name="answer"/> is null.</returns>
        public static QuestionResponseDTO ToDto(this QuestionResponse answer)
        {
            ArgumentNullException.ThrowIfNull(answer);
            return new QuestionResponseDTO
            {
                QuestionId = answer.QuestionId,
                Response = answer.Response
            };
        }

        /// <summary>
        /// Converts a <see cref="QuestionResponseDTO"/> to a <see cref="QuestionResponse"/> entity.
        /// </summary>
        /// <param name="dto">The data transfer object containing the question response details. Cannot be <see langword="null"/>.</param>
        /// <param name="questionnaireResponseId">The unique identifier for the questionnaire response associated with this question response.</param>
        /// <returns>A new <see cref="QuestionResponse"/> entity initialized with the data from the specified <paramref
        /// name="dto"/> and the provided <paramref name="questionnaireResponseId"/>.</returns>
        public static QuestionResponse ToEntity(this QuestionResponseDTO dto, Guid questionnaireResponseId)
        {
            ArgumentNullException.ThrowIfNull(dto);
            return new QuestionResponse
            {
                Id = Guid.NewGuid(),
                QuestionId = dto.QuestionId,
                Response  = dto.Response,
                QuestionnaireResponseId = questionnaireResponseId
            };
        }
    }
}
