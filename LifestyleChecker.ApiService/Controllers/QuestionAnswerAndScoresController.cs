using LifestyleChecker.ApiService.Attrbutes;
using LifestyleChecker.Domain.Models;
using LifestyleChecker.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LifestyleChecker.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionAnswerAndScoresController : ControllerBase
    {
        private readonly LifestyleCheckerDbContext _context;

        public QuestionAnswerAndScoresController(LifestyleCheckerDbContext context)
        {
            _context = context;
        }

        // GET: api/QuestionAnswerAndScores
        [HttpGet]
        [PatientAuthorise]
        [ProducesResponseType(typeof(IEnumerable<QuestionAnswerAndScore>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<QuestionAnswerAndScore>>> GetQuestionAnswerAndScores()
        {
            return await _context.QuestionAnswerAndScores.ToListAsync();
        }

        // GET: api/QuestionAnswerAndScores/5
        [HttpGet("{id}")]
        [PatientAuthorise]
        [ProducesResponseType(typeof(QuestionAnswerAndScore), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<QuestionAnswerAndScore>> GetQuestionAnswerAndScore(Guid id)
        {
            var questionAnswerAndScore = await _context.QuestionAnswerAndScores.FindAsync(id);

            if (questionAnswerAndScore == null)
            {
                return NotFound();
            }

            return questionAnswerAndScore;
        }

        // PUT: api/QuestionAnswerAndScores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [AdminAuthorise]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutQuestionAnswerAndScore(Guid id, QuestionAnswerAndScore questionAnswerAndScore)
        {
            if (id != questionAnswerAndScore.Id)
            {
                return BadRequest();
            }

            _context.Entry(questionAnswerAndScore).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionAnswerAndScoreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/QuestionAnswerAndScores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [AdminAuthorise]
        [ProducesResponseType(typeof(QuestionAnswerAndScore), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<QuestionAnswerAndScore>> PostQuestionAnswerAndScore(QuestionAnswerAndScore questionAnswerAndScore)
        {
            _context.QuestionAnswerAndScores.Add(questionAnswerAndScore);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestionAnswerAndScore", new { id = questionAnswerAndScore.Id }, questionAnswerAndScore);
        }

        // DELETE: api/QuestionAnswerAndScores/5
        [HttpDelete("{id}")]
        [AdminAuthorise]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteQuestionAnswerAndScore(Guid id)
        {
            var questionAnswerAndScore = await _context.QuestionAnswerAndScores.FindAsync(id);
            if (questionAnswerAndScore == null)
            {
                return NotFound();
            }

            _context.QuestionAnswerAndScores.Remove(questionAnswerAndScore);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuestionAnswerAndScoreExists(Guid id)
        {
            return _context.QuestionAnswerAndScores.Any(e => e.Id == id);
        }
    }
}
