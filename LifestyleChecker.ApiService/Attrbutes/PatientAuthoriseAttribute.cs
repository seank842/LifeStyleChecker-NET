using LifestyleChecker.ApiService.ClaimTypes;
using LifestyleChecker.Contracts.DTOs;
using LifestyleChecker.Infrastructure.Persistence;
using LifestyleChecker.Services.PatientService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System.CodeDom;
using System.Globalization;
using System.Security.Claims;
using System.Text;

namespace LifestyleChecker.ApiService.Attrbutes
{
    /// <summary>
    /// An attribute that performs authorization for patient access based on Basic Authentication credentials.
    /// </summary>
    /// <remarks>This attribute is used to authorize requests by validating the provided Basic Authentication
    /// credentials against patient records. It checks for the presence of an NHS number, surname, and date of birth in
    /// the credentials, and verifies them against a patient lookup service. If the credentials are invalid or the
    /// patient cannot be found, the request is denied with an appropriate HTTP status code.</remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PatientAuthoriseAttribute : Attribute, IAsyncAuthorizationFilter
    {
        /// <summary>
        /// Asynchronously handles authorization by validating basic authentication credentials against a patient lookup
        /// service.
        /// </summary>
        /// <remarks>This method checks for the presence of a patient lookup service and attempts to
        /// decode and validate basic authentication credentials. If the credentials are invalid or the patient cannot
        /// be found, it sets the result to an unauthorized response. If the patient lookup service is unavailable, it
        /// sets the result to an internal server error.</remarks>
        /// <param name="context">The authorization filter context, which provides access to the HTTP context and allows setting the result of
        /// the authorization process.</param>
        /// <returns></returns>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Try to resolve the patient lookup service
            if (context.HttpContext.RequestServices.GetService(typeof(IPatientLookup)) is not IPatientLookup patientLookup)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return;
            }
            
            if (!TryGetDecodedBasicAuthCredentials(context, out var decoded))
            {
                context.HttpContext.Response.Headers.WWWAuthenticate = "Basic";
                context.Result = new BadRequestObjectResult("Authorization header is missing or malformed.");
                return;
            }
            
            if (!TryParseCredentials(decoded, out var nHSNumber, out var surname, out var dateOfBirth))
            {
                context.HttpContext.Response.Headers.WWWAuthenticate = "Basic";
                context.Result = new BadRequestObjectResult("Authorization header is missing or malformed.");
                return;
            }

            // Lookup patient
            PatientDTO patient = null!;
            try
            {
                patient = await patientLookup.GetPatientByIdAsync(nHSNumber);
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Error retrieving patient with ID"))
                {
                    context.HttpContext.Response.Headers.WWWAuthenticate = "Basic";
                    context.Result = new NotFoundObjectResult("Your details could not be found");
                }
                else
                {
                    context.HttpContext.Response.Headers.WWWAuthenticate = "Basic";
                    context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
                return;
            }

            // Validate patient login
            var patientValidationReturn = PatientAuthenticationAndValidation.ValidatePatientLogin(patient, nHSNumber, surname, dateOfBirth);
            if (!patientValidationReturn.valid)
            {
                context.HttpContext.Response.Headers.WWWAuthenticate = "Basic";
                if(patientValidationReturn.message == "You are not eligble for this service")
                {
                    context.Result = new UnauthorizedObjectResult(patientValidationReturn.message);
                }
                else
                    // If the patient is not found or the details do not match, return unauthorized
                    context.Result = new NotFoundObjectResult(patientValidationReturn.message);
                return;
            }

            var claims = new List<Claim>
            {
                new Claim(CustomClaimTypes.NHSNumber, nHSNumber),
                new Claim(System.Security.Claims.ClaimTypes.Surname, surname),
                new Claim(System.Security.Claims.ClaimTypes.DateOfBirth, dateOfBirth.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
            };
            var identity = new ClaimsIdentity(claims, "Basic");
            context.HttpContext.User = new ClaimsPrincipal(identity);
        }


        /// <summary>
        /// Attempts to parse the provided decoded string into NHS number, surname, and date of birth.
        /// </summary>
        /// <remarks>The method expects the input string to be in the format
        /// "NHSNumber|Surname:DateOfBirth", where the date of birth is in "dd-MM-yyyy" format.</remarks>
        /// <param name="decoded">The decoded string containing the credentials in the format "NHSNumber|Surname:DateOfBirth".</param>
        /// <param name="nHSNumber">When this method returns, contains the parsed NHS number if the parsing succeeded; otherwise, <see
        /// langword="null"/>.</param>
        /// <param name="surname">When this method returns, contains the parsed surname if the parsing succeeded; otherwise, <see
        /// langword="null"/>.</param>
        /// <param name="dateOfBirth">When this method returns, contains the parsed date of birth if the parsing succeeded; otherwise, the default
        /// value for <see cref="DateTime"/>.</param>
        /// <returns><see langword="true"/> if the string was successfully parsed; otherwise, <see langword="false"/>.</returns>
        private static bool TryParseCredentials(string decoded, out string nHSNumber, out string surname, out DateTime dateOfBirth)
        {
            nHSNumber = null;
            surname = null;
            dateOfBirth = default;

            var parts = decoded.Split(':', 2);
            if (parts.Length != 2)
                return false;

            var nHSNumberAndSurname = parts[0].Split('|', 2);
            if (nHSNumberAndSurname.Length != 2)
                return false;

            nHSNumber = nHSNumberAndSurname[0];
            surname = nHSNumberAndSurname[1];

            // Remove time component if present in the date string
            var datePart = parts[1].Split(' ', 2)[0];

            // With this corrected version to support both "dd-MM-yyyy" and "dd/MM/yyyy" formats:
            if (!DateTime.TryParseExact(datePart, ["dd/MM/yyyy", "dd-MM-yyyy"], CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth))
                return false;

            return true;
        }

        /// <summary>
        /// Attempts to decode the Basic Authentication credentials from the HTTP request's Authorization header.
        /// </summary>
        /// <remarks>This method checks if the Authorization header is present and starts with "Basic ".
        /// If so, it attempts to decode the credentials from Base64. If the header is missing, improperly formatted, or
        /// the Base64 decoding fails, the method returns <see langword="false"/>.</remarks>
        /// <param name="context">The authorization filter context containing the HTTP request.</param>
        /// <param name="decoded">When this method returns, contains the decoded credentials if the operation was successful; otherwise, <see
        /// langword="null"/>.</param>
        /// <returns><see langword="true"/> if the Authorization header contains valid Base64-encoded Basic Authentication
        /// credentials; otherwise, <see langword="false"/>.</returns>
        private static bool TryGetDecodedBasicAuthCredentials(AuthorizationFilterContext context, out string decoded)
        {
            decoded = null;
            var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            string encoded = authHeader.Substring("Basic ".Length).Trim();
            try
            {
                decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
