using LifestyleChecker.ApiService.Attrbutes;
using LifestyleChecker.ApiService.ClaimTypes;
using LifestyleChecker.Contracts.DTOs;
using LifestyleChecker.Services.PatientService;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LifestyleChecker.ApiService.Controllers
{
    /// <summary>
    /// Provides API endpoints for user-related operations, specifically for retrieving the current user's information.
    /// </summary>
    /// <remarks>The <see cref="UserController"/> class is responsible for handling HTTP requests related to
    /// user data. It utilizes the <see cref="IPatientLookup"/> service to fetch patient information based on the user's
    /// NHS Number claim.</remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IPatientLookup _patientLookup;
        public UserController(IPatientLookup patientLookup)
        {
            _patientLookup = patientLookup ?? throw new ArgumentNullException(nameof(patientLookup), "Patient lookup service cannot be null.");
        }

        
        /// <summary>
        /// Retrieves the current user's information based on their NHS Number claim.
        /// </summary>
        /// <remarks>This method attempts to find the current user's NHS Number from the claims and
        /// retrieves the corresponding patient information. If the NHS Number is not present in the claims, the method
        /// returns an unauthorized response. If no patient is found with the given NHS Number, a not found response is
        /// returned. In case of an error during the retrieval process, an internal server error response is
        /// returned.</remarks>
        /// <returns>An <see cref="ActionResult{T}"/> containing the patient information as a string if found;  otherwise, an
        /// appropriate HTTP error response.</returns>
        [HttpGet("Me")]
        [PatientAuthorise]
        [ProducesResponseType(typeof(PatientDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PatientDTO>> GetCurrentUser()
        {
            var nHSNumber = User.FindFirstValue(CustomClaimTypes.NHSNumber);
            if (string.IsNullOrEmpty(nHSNumber))
            {
                return Unauthorized("NHS Number not found in claims.");
            }
            try
            {
                var patient = await _patientLookup.GetPatientByIdAsync(nHSNumber);
                if (patient == null)
                {
                    return NotFound($"Patient with NHS Number {nHSNumber} not found.");
                }
                return Ok(patient);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving user information: {ex.Message}");
            }
        }

        /// <summary>
        /// Verifies if the current user has administrative privileges.
        /// </summary>
        /// <remarks>This method checks the authorization status of the user and returns a 204 No Content
        /// response if the user is authorized as an administrator. It requires the user to be authenticated with
        /// administrative credentials.</remarks>
        /// <returns>An <see cref="ActionResult"/> indicating the authorization status. Returns 204 No Content if the user is
        /// authorized.</returns>
        [HttpGet("AdminAuthCheck")]
        [AdminAuthorise]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult AdminAuthCheck()
        {
            return NoContent();
        }
    }
}
