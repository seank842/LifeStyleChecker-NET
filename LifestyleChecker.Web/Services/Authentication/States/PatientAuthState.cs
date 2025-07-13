namespace LifestyleChecker.Web.Services.Authentication.States
{
    /// <summary>
    /// Represents the authentication state of a patient, including their NHS number, surname, and date of birth.
    /// </summary>
    /// <remarks>This class provides properties to manage and track the authentication state of a patient. It
    /// includes functionality to notify subscribers when the authentication state changes and to generate a basic
    /// authentication header if the patient is authenticated.</remarks>
    public class PatientAuthState
    {
        /// <summary>
        /// Occurs when the authentication state changes.
        /// </summary>
        /// <remarks>Subscribe to this event to be notified whenever there is a change in the
        /// authentication state. This can be useful for updating UI elements or triggering other actions in response to
        /// authentication changes.</remarks>
        public event Action? AuthStateChanged;

        private string? _nhsNumber;
        private string? _surname;
        private DateTime? _dateOfBirth;

        /// <summary>
        /// Gets or sets the NHS (National Health Service) number associated with the patient.
        /// </summary>
        /// <remarks>Changing the NHS number triggers an authentication state change
        /// notification.</remarks>
        public string? NHSNumber
        {
            get => _nhsNumber;
            set
            {
                if (_nhsNumber != value)
                {
                    _nhsNumber = value;
                    NotifyAuthStateChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the surname of the user.
        /// </summary>
        /// <remarks>Changing the Surname triggers an authentication state change
        /// notification.</remarks>
        public string? Surname
        {
            get => _surname;
            set
            {
                if (_surname != value)
                {
                    _surname = value;
                    NotifyAuthStateChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        /// <remarks>Changing the Date of Birth triggers an authentication state change
        /// notification.</remarks>
        public DateTime? DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                if (_dateOfBirth != value)
                {
                    _dateOfBirth = value;
                    NotifyAuthStateChanged();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user is authenticated.
        /// </summary>
        public bool IsAuthenticated => 
            !string.IsNullOrEmpty(NHSNumber) && 
            !string.IsNullOrEmpty(Surname) && 
            DateOfBirth != null;

        /// <summary>
        /// Generates a Basic Authentication header for the current user.
        /// </summary>
        /// <remarks>The method constructs the header using the user's NHS number, surname, and date of
        /// birth. The header is returned in the format required for HTTP Basic Authentication.</remarks>
        /// <returns>A string containing the Basic Authentication header if the user is authenticated; otherwise, <see
        /// langword="null"/>.</returns>
        public string? GetBasicAuthHeader()
        {
            if (!IsAuthenticated) return null;
            var credentials = $"{NHSNumber}|{Surname}:{DateOfBirth}";
            return "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));
        }

        /// <summary>
        /// Clears all personal information fields by setting them to <see langword="null"/>.
        /// </summary>
        /// <remarks>This method resets the NHS number, surname, and date of birth fields to their default
        /// state. Use this method to remove all stored personal information from the current instance.</remarks>
        public void Clear()
        {
            NHSNumber = null;
            Surname = null;
            DateOfBirth = null;
        }

        /// <summary>
        /// Notifies subscribers that the authentication state has changed.
        /// </summary>
        /// <remarks>This method triggers the <see cref="AuthStateChanged"/> event, allowing subscribers
        /// to respond to changes in authentication state.</remarks>
        private void NotifyAuthStateChanged()
        {
            AuthStateChanged?.Invoke();
        }
    }
}
