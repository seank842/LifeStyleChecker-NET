using LifestyleChecker.ApiService.Attrbutes;
using LifestyleChecker.ApiService.ClaimTypes;
using LifestyleChecker.ApiService.Mapping;
using LifestyleChecker.ApiService.Services;
using LifestyleChecker.Contracts.DTOs;
using LifestyleChecker.Infrastructure.Persistence;
using LifestyleChecker.Service.Scoring;
using LifestyleChecker.SharedUtilities.Converters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LifestyleChecker.ApiService.Controllers
{
    /// <summary>
    /// Provides endpoints for evaluating and retrieving questionnaire responses for patients.
    /// </summary>
    /// <remarks>The <see cref="EvaluationController"/> class handles HTTP requests related to the evaluation
    /// of questionnaire responses. It supports operations to retrieve evaluations for the authenticated patient and to
    /// evaluate a new questionnaire response. The controller uses caching to optimize retrieval operations and requires
    /// patient authentication to access its endpoints.</remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluationController : ControllerBase
    {
        private readonly LifestyleCheckerDbContext _context;
        private readonly IScoringService _scoringService;
        private readonly ICacheService _cacheService;
        private const string _cacheKey = "evaluations";

        public EvaluationController(LifestyleCheckerDbContext context, IScoringService scoringService, ICacheService cacheService)
        {
            _context = context;
            _scoringService = scoringService;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Retrieves a list of evaluations for the authenticated patient.
        /// </summary>
        /// <remarks>This method fetches evaluations based on the patient's NHS number, which is extracted
        /// from the user's claims. If evaluations are available in the cache, they are returned directly. Otherwise,
        /// evaluations are computed from the patient's questionnaire responses and then cached for future
        /// requests.</remarks>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="EvaluationDTO"/> representing the evaluations for the patient.
        /// Returns an empty list if no questionnaire responses are found.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown if the NHS number is not found in the user's claims.</exception>
        [HttpGet]
        [PatientAuthorise]
        [ProducesResponseType(typeof(IEnumerable<EvaluationDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<EvaluationDTO>>> GetEvaluations()
        {
            var nhsNumber = User.FindFirstValue(CustomClaimTypes.NHSNumber);
            if (string.IsNullOrEmpty(nhsNumber))
                throw new UnauthorizedAccessException("NHS Number not found in claims.");

            // Try to get cached evaluations
            var cachedEvaluations = await _cacheService.GetAsync<List<EvaluationDTO>>($"{_cacheKey}:{nhsNumber}");
            if (cachedEvaluations != null)
            {
                return Ok(cachedEvaluations);
            }

            var questionnaireResponses = await _context.QuestionnaireResponses
                .Include(qr => qr.Questionnaire)
                    .ThenInclude(q => q.EvaluationCriteria)
                .Include(qr => qr.Questionnaire)
                    .ThenInclude(q => q.Questions)
                        .ThenInclude(q => q.Answers)
                            .ThenInclude(a => a.AgeGroup)
                .Include(qr => qr.QuestionResponses)
                .Where(qr => qr.RespondentNHSNumber == nhsNumber)
                .ToListAsync();
            if (!questionnaireResponses.Any())
            {
                return Ok(new List<EvaluationDTO>());
            }
            var evaluations = questionnaireResponses.Select(qr =>
                _scoringService.EvaluateResults(qr.Questionnaire, qr)).ToList();

            // Cache the evaluations for 10 minutes
            await _cacheService.SetAsync($"{_cacheKey}:{nhsNumber}", evaluations, TimeSpan.FromMinutes(10));

            return Ok(evaluations);
        }

        /// <summary>
        /// Evaluates a questionnaire response and returns the evaluation results.
        /// </summary>
        /// <remarks>This method processes the provided questionnaire response, associates it with the
        /// respondent's NHS number, and evaluates the results based on predefined criteria. The method requires the
        /// respondent's NHS number and date of birth to be present in the user's claims.</remarks>
        /// <param name="questionnaireResponse">The response to the questionnaire, containing the answers provided by the respondent.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing an <see cref="EvaluationDTO"/> with the evaluation results if
        /// successful; otherwise, a bad request response.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown if the NHS number is not found in the user's claims.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the date of birth is not found in the user's claims.</exception>
        [HttpPost]
        [PatientAuthorise]
        [ProducesResponseType(typeof(EvaluationDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EvaluationDTO>> Evaluate(QuestionnaireResponseDTO questionnaireResponse)
        {
            if (questionnaireResponse == null || questionnaireResponse.QuestionResponses == null || !questionnaireResponse.QuestionResponses.Any())
            {
                return BadRequest("Invalid questionnaire response.");
            }
            var nhsNumber = User.FindFirstValue(CustomClaimTypes.NHSNumber);
            if (string.IsNullOrEmpty(nhsNumber))
                throw new UnauthorizedAccessException("NHS Number not found in claims.");
            var questionnaireResponseEntity = questionnaireResponse.ToEntity();
            questionnaireResponseEntity.RespondentNHSNumber = nhsNumber;
            var dateOfBirthClaim = User.FindFirstValue(System.Security.Claims.ClaimTypes.DateOfBirth);
            if (string.IsNullOrEmpty(dateOfBirthClaim))
            {
                throw new InvalidOperationException("Date of Birth not found in claims.");
            }
            questionnaireResponseEntity.AgeGroupAtTimeOfResponse = DateTime.Parse(dateOfBirthClaim).ToAge();
            _context.QuestionnaireResponses.Add(questionnaireResponseEntity);
            await _context.SaveChangesAsync();

            await _cacheService.RemoveAsync($"{_cacheKey}:{nhsNumber}");

            var questionnaire = await _context.Questionnaires
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Answers)
                        .ThenInclude(a=>a.AgeGroup)
                .Include(q => q.EvaluationCriteria)
                .Include(q => q.QuestionnaireResponses.Where(qr => qr.RespondentNHSNumber == nhsNumber)
                                                      .OrderBy(qr => qr.CreatedAt))
                    .ThenInclude(qr => qr.QuestionResponses)
                .FirstOrDefaultAsync(q => q.Id == questionnaireResponse.QuestionnaireId);

            if(questionnaire == null)
            {
                return BadRequest("Questionnaire not found.");
            }

            return Ok(_scoringService.EvaluateResults(questionnaire, questionnaire.QuestionnaireResponses.Last()));
        }
    }
}
