using LifestyleChecker.Contracts.DTOs;
using LifestyleChecker.Domain.Models;

namespace LifestyleChecker.ApiService.Mapping
{
    /// <summary>
    /// Provides methods for converting between <see cref="QuestionAnswerAndScore"/> and <see cref="QuestionAnswerDTO"/>
    /// objects.
    /// </summary>
    /// <remarks>This static class includes methods to facilitate the conversion of question and answer data
    /// between different representations. Note that not all conversions are supported; see individual method
    /// documentation for details.</remarks>
    public static class QuestionAnswerMapping
    {
        /// <summary>
        /// Converts a <see cref="QuestionAnswerAndScore"/> object to a <see cref="QuestionAnswerDTO"/> object.
        /// </summary>
        /// <param name="questionAnswer">The <see cref="QuestionAnswerAndScore"/> instance to convert. Cannot be null.</param>
        /// <returns>A <see cref="QuestionAnswerDTO"/> object containing the converted data, or <see langword="null"/> if
        /// <paramref name="questionAnswer"/> is <see langword="null"/>.</returns>
        public static QuestionAnswerDTO ToDto(this QuestionAnswerAndScore questionAnswer)
        {
            ArgumentNullException.ThrowIfNull(questionAnswer);
            return new QuestionAnswerDTO
            {
                AnswerOption = questionAnswer.Answer,
                Order = questionAnswer.Order,
            };
        }
        /// <summary>
        /// Converts a <see cref="QuestionAnswerDTO"/> to a <see cref="QuestionAnswerAndScore"/> entity.
        /// </summary>
        /// <remarks>This method is not supported and will throw a <see cref="NotSupportedException"/>. 
        /// Use the <see cref="QuestionAnswerAndScore"/> constructor instead.</remarks>
        /// <param name="dto">The <see cref="QuestionAnswerDTO"/> instance to convert.</param>
        /// <returns>This method does not return a value as it is not supported.</returns>
        /// <exception cref="NotSupportedException">Always thrown to indicate that this method is not supported.</exception>
        public static QuestionAnswerAndScore ToEntity(this QuestionAnswerDTO dto)
        {
            throw new NotSupportedException("This method is not supported. Use QuestionAnswerAndScore constructor instead.");
        }
    }
}
