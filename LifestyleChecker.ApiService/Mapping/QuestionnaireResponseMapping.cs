using LifestyleChecker.Contracts.DTOs;
using LifestyleChecker.Domain.Models;

namespace LifestyleChecker.ApiService.Mapping
{
    /// <summary>
    /// Provides extension methods for mapping between <see cref="QuestionnaireResponse"/> and <see
    /// cref="QuestionnaireResponseDTO"/> objects.
    /// </summary>
    /// <remarks>This static class includes methods to convert entities to DTOs and vice versa, facilitating
    /// data transfer between different layers of an application.</remarks>
    public static class QuestionnaireResponseMapping
    {
        /// <summary>
        /// Converts the specified <see cref="QuestionnaireResponse"/> to a <see cref="QuestionnaireResponseDTO"/>.
        /// </summary>
        /// <param name="response">The <see cref="QuestionnaireResponse"/> to convert.</param>
        /// <returns>A <see cref="QuestionnaireResponseDTO"/> representation of the specified response.</returns>
        /// <exception cref="NotSupportedException">This operation is not supported on this type.</exception>
        public static QuestionnaireResponseDTO ToDto(this QuestionnaireResponse response)
        {
            throw new NotSupportedException("This operation is not supported on this type.");
        }

        /// <summary>
        /// Converts a <see cref="QuestionnaireResponseDTO"/> to a <see cref="QuestionnaireResponse"/> entity.
        /// </summary>
        /// <param name="dto">The data transfer object to convert. Cannot be null.</param>
        /// <returns>A <see cref="QuestionnaireResponse"/> entity populated with data from the specified DTO, or null if the DTO
        /// is null.</returns>
        public static QuestionnaireResponse ToEntity(this QuestionnaireResponseDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            return new QuestionnaireResponse
            {
                QuestionnaireId = dto.QuestionnaireId,
                QuestionResponses = dto.QuestionResponses?.Select(qr => qr.ToEntity(dto.QuestionnaireId)).ToList() ?? new List<QuestionResponse>(),
            };
        }
    }
}
