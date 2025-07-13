namespace LifestyleChecker.Web.Services.Authentication.States
{
    public class AdminAuthState
    {
        public event Action? AuthStateChanged;

        private string? _username;
        private string? _password;

        /// <summary>
        /// Gets or sets the username associated with the current user session.
        /// </summary>
        /// <remarks>Changing the Username triggers an authentication state change
        /// notification.</remarks>
        public string? Username
        {
            get => _username;
            set
            {
                if(_username != value)
                {
                    _username = value;
                    NotifyAuthStateChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the password used for authentication.
        /// </summary>
        /// <remarks>Changing the Password triggers an authentication state change
        /// notification.</remarks>
        public string? Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    NotifyAuthStateChanged();
                }
            }
        }
        
        /// <summary>
        /// Gets a value indicating whether the user is authenticated.
        /// </summary>
        public bool IsAuthenticated => 
            !string.IsNullOrEmpty(Username) &&
            !string.IsNullOrEmpty(Password);

        /// <summary>
        /// Generates a Basic Authentication header value for the current user credentials.
        /// </summary>
        /// <remarks>This method returns a Base64-encoded string suitable for use as a Basic
        /// Authentication header. The method returns <see langword="null"/> if the user is not authenticated.</remarks>
        /// <returns>A string representing the Basic Authentication header value, or <see langword="null"/> if the user is not
        /// authenticated.</returns>
        public string? GetBasicAuthHeader()
        {
            if (!IsAuthenticated) return null;
            var credentials = $"{Username}:{Password}";
            return "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));
        }

        /// <summary>
        /// Clears the stored username and password.
        /// </summary>
        /// <remarks>This method sets the <see cref="Username"/> and <see cref="Password"/> properties to
        /// <see langword="null"/>, effectively removing any stored credentials.</remarks>
        public void Clear()
        {
            Username = null;
            Password = null;
        }

        /// <summary>
        /// Notifies subscribers that the authentication state has changed.
        /// </summary>
        /// <remarks>This method triggers the <see cref="AuthStateChanged"/> event, allowing subscribers
        /// to respond to changes in authentication state. Ensure that any event handlers attached to <see
        /// cref="AuthStateChanged"/> are prepared to handle the invocation.</remarks>
        private void NotifyAuthStateChanged()
        {
            AuthStateChanged?.Invoke();
        }
    }
}
