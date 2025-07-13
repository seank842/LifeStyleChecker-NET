using LifestyleChecker.Web.Services.Authentication.States;
using System.Net.Http.Headers;

namespace LifestyleChecker.Web.Services.Authentication
{
    public class AdminAuthHandler : DelegatingHandler
    {
        private readonly AdminAuthState _authState;

        public AdminAuthHandler(AdminAuthState authState)
        {
            _authState = authState ?? throw new ArgumentNullException(nameof(authState));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var header = _authState.GetBasicAuthHeader();
            if (!string.IsNullOrEmpty(header))
            {
                request.Headers.Authorization = AuthenticationHeaderValue.Parse(header);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
