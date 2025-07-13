using LifestyleChecker.Contracts.DTOs;
using LifestyleChecker.Domain.Models;

namespace LifestyleChecker.ApiService.Mapping
{
    /// <summary>
    /// Provides extension methods for converting between <see cref="Questionnaire"/> and <see cref="QuestionnaireDTO"/>
    /// objects.
    /// </summary>
    /// <remarks>This static class includes methods to facilitate the transformation of questionnaire data
    /// between domain entities and data transfer objects, ensuring that the data can be easily mapped for different
    /// layers of an application.</remarks>
    public static class QuestionnaireMapping
    {
        /// <summary>
        /// Converts a <see cref="Questionnaire"/> object to a <see cref="QuestionnaireDTO"/> object.
        /// </summary>
        /// <param name="questionnaire">The <see cref="Questionnaire"/> instance to convert. Cannot be null.</param>
        /// <returns>A <see cref="QuestionnaireDTO"/> representing the converted questionnaire, or <see langword="null"/> if the
        /// input is <see langword="null"/>.</returns>
        public static QuestionnaireDTO ToDto(this Questionnaire questionnaire)
        {
            ArgumentNullException.ThrowIfNull(questionnaire);
            return new QuestionnaireDTO
            {
                Id = questionnaire.Id,
                Name = questionnaire.Name,
                Version = questionnaire.Version,
                Questions = questionnaire.Questions?.Select(q => q.ToDto()).ToList() ?? new List<QuestionDTO>()
            };
        }

        /// <summary>
        /// Converts a <see cref="QuestionnaireDTO"/> to a <see cref="Questionnaire"/> entity.
        /// </summary>
        /// <param name="dto">The <see cref="QuestionnaireDTO"/> instance to convert. Cannot be null.</param>
        /// <returns>A <see cref="Questionnaire"/> entity populated with data from the <paramref name="dto"/>. Returns null if
        /// <paramref name="dto"/> is null.</returns>
        public static Questionnaire ToEntity(this QuestionnaireDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            return new Questionnaire
            {
                Id = dto.Id,
                Name = dto.Name,
                Version = dto.Version,
                Questions = dto.Questions?.Select(q => q.ToEntity()).ToList() ?? new List<QuestionnaireQuestion>()
            };
        }
    }
}
