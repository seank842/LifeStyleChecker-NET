using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LifestyleChecker.Domain.Models;
using LifestyleChecker.Infrastructure.Persistence;
using LifestyleChecker.ApiService.Attrbutes;
using LifestyleChecker.Contracts.DTOs;
using LifestyleChecker.ApiService.Mapping;
using LifestyleChecker.ApiService.Services;

namespace LifestyleChecker.ApiService.Controllers
{
    /// <summary>
    /// Provides API endpoints for managing questionnaires, including retrieval, creation, and deletion operations.
    /// </summary>
    /// <remarks>This controller supports operations for retrieving collections of questionnaires, fetching
    /// individual questionnaires by ID, creating new questionnaires, and deleting existing ones. It utilizes caching to
    /// enhance performance and requires appropriate authorization for certain operations.</remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionnairesController : ControllerBase
    {
        private readonly LifestyleCheckerDbContext _context;
        private readonly ICacheService _cacheService;

        public QuestionnairesController(LifestyleCheckerDbContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Retrieves a collection of questionnaires, optionally including all versions.
        /// </summary>
        /// <remarks>The method utilizes caching to improve performance. If the requested data is
        /// available in the cache, it is retrieved from there; otherwise, it is fetched from the database and then
        /// cached for future requests.</remarks>
        /// <param name="AllVersions">A boolean value indicating whether to include all versions of each questionnaire. If <see
        /// langword="false"/>, only the latest version of each questionnaire is returned.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="IEnumerable{T}"/> of <see
        /// cref="QuestionnaireDTO"/> objects. Returns a 200 OK status code with the list of questionnaires.</returns>
        [HttpGet]
        [PatientAuthorise]
        [ProducesResponseType(typeof(IEnumerable<QuestionnaireDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<QuestionnaireDTO>>> GetQuestionnaires([FromQuery] bool AllVersions = false)
        {
            string _cacheKey = $"questionnaires:all:allVersions-{AllVersions}";
            var cached = await _cacheService.GetAsync<List<Questionnaire>>(_cacheKey);
            IEnumerable<QuestionnaireDTO> questionnaireDtos = new List<QuestionnaireDTO>();
            if (cached != null)
            {
                questionnaireDtos = cached.Select(QuestionnaireMapping.ToDto).ToList();
            }
            else
            {
                var questionnaires = await _context.Questionnaires.ToListAsync();
                if (!AllVersions)
                {
                    questionnaires = questionnaires
                        .GroupBy(q => q.Name)
                        .Select(g => g.OrderByDescending(q => q.Version).First())
                        .ToList();
                }
                await _cacheService.SetAsync(_cacheKey, questionnaires, TimeSpan.FromMinutes(30));
                questionnaireDtos = questionnaires.Select(QuestionnaireMapping.ToDto).ToList();
            }
            return Ok(questionnaireDtos);
        }

        /// <summary>
        /// Retrieves a questionnaire by its unique identifier.
        /// </summary>
        /// <remarks>The method first attempts to retrieve the questionnaire from the cache. If not found
        /// in the cache,  it queries the database. The result is cached for subsequent requests.</remarks>
        /// <param name="id">The unique identifier of the questionnaire to retrieve.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="QuestionnaireDTO"/> if found;  otherwise, a 404 Not
        /// Found status.</returns>
        [HttpGet("{id}")]
        [PatientAuthorise]
        [ProducesResponseType(typeof(Questionnaire), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<QuestionnaireDTO>> GetQuestionnaire(Guid id)
        {
            string cacheKey = $"questionnaire:{id}";
            var cachedQuestionnaire = await _cacheService.GetAsync<Questionnaire>(cacheKey);
            QuestionnaireDTO? questionnaireDTO;
            if (cachedQuestionnaire != null)
                questionnaireDTO = cachedQuestionnaire.ToDto();
            else
            {
                var questionnaire = await _context.Questionnaires
                                                  .Include(q => q.Questions)
                                                  .ThenInclude(q => q.Answers)
                                                  .FirstOrDefaultAsync(q => q.Id == id);
                if (questionnaire != null)
                    await _cacheService.SetAsync(cacheKey, questionnaire, TimeSpan.FromMinutes(30));
                else
                    return NotFound();
                questionnaireDTO = questionnaire?.ToDto();
            }
            if (questionnaireDTO == null)
            {
                return NotFound();
            }

            return questionnaireDTO;
        }

        /// <summary>
        /// Creates a new questionnaire and stores it in the database.
        /// </summary>
        /// <remarks>This method requires administrative authorization. Upon successful creation, the
        /// cache for all questionnaires is invalidated.</remarks>
        /// <param name="questionnaire">The data transfer object containing the details of the questionnaire to be created. Cannot be null.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing the created <see cref="QuestionnaireDTO"/> and a status code of
        /// 201 (Created).</returns>
        [HttpPost]
        [AdminAuthorise]
        [ProducesResponseType(typeof(Questionnaire), StatusCodes.Status201Created)]
        public async Task<ActionResult<QuestionnaireDTO>> PostQuestionnaire(QuestionnaireDTO questionnaire)
        {
            _context.Questionnaires.Add(questionnaire.ToEntity());
            await _context.SaveChangesAsync();
            await _cacheService.RemoveAsync($"questionnaires:all:*");

            return CreatedAtAction("GetQuestionnaire", new { id = questionnaire.Id }, questionnaire);
        }

        /// <summary>
        /// Deletes the questionnaire with the specified identifier.
        /// </summary>
        /// <remarks>This operation requires administrative authorization. Upon successful deletion,  the
        /// method also clears related cache entries to ensure data consistency.</remarks>
        /// <param name="id">The unique identifier of the questionnaire to delete.</param>
        /// <returns>A <see cref="NoContentResult"/> if the questionnaire is successfully deleted;  otherwise, a <see
        /// cref="NotFoundResult"/> if the questionnaire does not exist.</returns>
        [HttpDelete("{id}")]
        [AdminAuthorise]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteQuestionnaire(Guid id)
        {
            var questionnaire = await _context.Questionnaires.FindAsync(id);
            if (questionnaire == null)
            {
                return NotFound();
            }
            _context.Questionnaires.Remove(questionnaire);
            await _context.SaveChangesAsync();
            await _cacheService.RemoveAsync($"questionnaires:all:*");
            await _cacheService.RemoveAsync($"questionnaire:{id}");
            return NoContent();
        }
    }
}
