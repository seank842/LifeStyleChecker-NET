using LifestyleChecker.Contracts.DTOs;
using LifestyleChecker.Services.PatientService.Structs;
using LifestyleChecker.SharedUtilities.Converters;

namespace LifestyleChecker.Services.PatientService
{
    /// <summary>
    /// Provides functionality to validate patient login information.
    /// </summary>
    public static class PatientAuthenticationAndValidation
    {
        /// <summary>
        /// Validates the login credentials of a patient by comparing the provided username, surname, and date of birth
        /// with the corresponding details in the <see cref="PatientDTO"/> object.
        /// </summary>
        /// <param name="patient">The <see cref="PatientDTO"/> object containing the patient's details to validate against.</param>
        /// <param name="username">The username provided for login. This parameter is currently not used in the validation process.</param>
        /// <param name="surname">The surname provided for login, which is compared to the patient's surname.</param>
        /// <param name="dateOfBirth">The date of birth provided for login, which is compared to the patient's date of birth.</param>
        /// <returns><see langword="true"/> if the provided surname and date of birth match the patient's details; otherwise,
        /// <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="patient"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if the patient's surname is null or empty.</exception>
        public static PatientLoginReturn ValidatePatientLogin(PatientDTO patient, string nHSNumber, string surname, DateTime dateOfBirth)
        {
            if (patient == null)
            {
                throw new ArgumentNullException(nameof(patient), "Patient data cannot be null.");
            }
            // Validate the patient's surname and date of birth
            if (string.IsNullOrEmpty(patient.Surname))
            {
                throw new ArgumentException("Patient name is invalid.");
            }
            if (string.Equals(patient.Surname, surname, StringComparison.OrdinalIgnoreCase) &&
                patient.DateOfBirth.Date.Equals(dateOfBirth.Date))
            {
                if (patient.DateOfBirth.ToAge() < 16)
                {
                    return new PatientLoginReturn(false, "You are not eligble for this service");
                }
                return new PatientLoginReturn(true, "");
            }
            return new PatientLoginReturn(false, "Your details could not be found");
        }
    }
}
