using LifestyleChecker.Contracts.DTOs;
using LifestyleChecker.Domain.Models;

namespace LifestyleChecker.ApiService.Mapping
{
    /// <summary>
    /// Provides methods for converting between <see cref="QuestionnaireQuestion"/> entities and <see
    /// cref="QuestionDTO"/> data transfer objects.
    /// </summary>
    /// <remarks>This static class offers extension methods to facilitate the transformation of question data
    /// between different layers of an application. It includes methods to convert a <see cref="QuestionnaireQuestion"/>
    /// to a <see cref="QuestionDTO"/> and vice versa, ensuring that all relevant properties are mapped
    /// appropriately.</remarks>
    public static class QuestionMapping
    {
        /// <summary>
        /// Converts a <see cref="QuestionnaireQuestion"/> to a <see cref="QuestionDTO"/>.
        /// </summary>
        /// <remarks>This method maps the properties of <see cref="QuestionnaireQuestion"/> to a new <see
        /// cref="QuestionDTO"/> instance. If the <paramref name="question"/> contains answer options, they are grouped
        /// by the answer text and converted to a list of <see cref="QuestionAnswerDTO"/>.</remarks>
        /// <param name="question">The <see cref="QuestionnaireQuestion"/> to convert. Cannot be null.</param>
        /// <returns>A <see cref="QuestionDTO"/> representing the converted question, or <see langword="null"/> if the input
        /// question is <see langword="null"/>.</returns>
        public static QuestionDTO ToDto(this QuestionnaireQuestion question)
        {
            ArgumentNullException.ThrowIfNull(question);
            return new QuestionDTO
            {
                Id = question.Id,
                QuestionText = question.QuestionText,
                AnswerType = question.AnswerType,
                Order = question.Order,
                IsRequired = question.IsRequired,
                AnswerOptions = question.Answers?
                    .GroupBy(a => a.Answer)
                    .Select(g => new QuestionAnswerDTO
                    {
                        AnswerOption = g.Key,
                        Order = g.First().Order
                    })
                    .ToList() ?? [],
                QuestionnaireId = question.QuestionnaireId
            };
        }

        /// <summary>
        /// Converts a <see cref="QuestionDTO"/> to a <see cref="QuestionnaireQuestion"/> entity.
        /// </summary>
        /// <param name="dto">The data transfer object containing question details. Cannot be null.</param>
        /// <returns>A <see cref="QuestionnaireQuestion"/> entity populated with data from the <paramref name="dto"/>. Returns
        /// null if <paramref name="dto"/> is null.</returns>
        public static QuestionnaireQuestion ToEntity(this QuestionDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            return new QuestionnaireQuestion
            {
                Id = dto.Id,
                QuestionText = dto.QuestionText,
                AnswerType = dto.AnswerType,
                Order = dto.Order,
                IsRequired = dto.IsRequired,
                QuestionnaireId = dto.QuestionnaireId
            };
        }
    }
}
