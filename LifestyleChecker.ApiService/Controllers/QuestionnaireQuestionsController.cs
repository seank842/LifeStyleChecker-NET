using LifestyleChecker.ApiService.Attrbutes;
using LifestyleChecker.ApiService.Mapping;
using LifestyleChecker.Contracts.DTOs;
using LifestyleChecker.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LifestyleChecker.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionnaireQuestionsController : ControllerBase
    {
        private readonly LifestyleCheckerDbContext _context;

        public QuestionnaireQuestionsController(LifestyleCheckerDbContext context)
        {
            _context = context;
        }

        // GET: api/QuestionnaireQuestions
        [HttpGet]
        [PatientAuthorise]
        [ProducesResponseType(typeof(IEnumerable<QuestionDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<QuestionDTO>>> GetQuestionnaireQuestions()
        {
            var questions = await _context.QuestionnaireQuestions
                .Include(q => q.Answers)
                .ToListAsync();

            var questionDtos = questions.Select(QuestionMapping.ToDto).ToList();

            return Ok(questionDtos);
        }

        // GET: api/QuestionnaireQuestions/5
        [HttpGet("{id}")]
        [PatientAuthorise]
        [ProducesResponseType(typeof(QuestionDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<QuestionDTO>> GetQuestionnaireQuestion(Guid id)
        {
            var questionnaireQuestion = await _context.QuestionnaireQuestions.FindAsync(id);

            if (questionnaireQuestion == null)
            {
                return NotFound();
            }

            return questionnaireQuestion.ToDto();
        }

        // POST: api/QuestionnaireQuestions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [AdminAuthorise]
        [ProducesResponseType(typeof(QuestionDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<QuestionDTO>> PostQuestionnaireQuestion(QuestionDTO questionnaireQuestion)
        {
            _context.QuestionnaireQuestions.Add(questionnaireQuestion.ToEntity());
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestionnaireQuestion", new { id = questionnaireQuestion.Id }, questionnaireQuestion);
        }

        // DELETE: api/QuestionnaireQuestions/5
        [HttpDelete("{id}")]
        [AdminAuthorise]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteQuestionnaireQuestion(Guid id)
        {
            var questionnaireQuestion = await _context.QuestionnaireQuestions.FindAsync(id);
            if (questionnaireQuestion == null)
            {
                return NotFound();
            }

            _context.QuestionnaireQuestions.Remove(questionnaireQuestion);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
