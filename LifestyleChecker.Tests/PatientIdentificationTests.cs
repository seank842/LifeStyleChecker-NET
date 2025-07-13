using Aspire.Hosting;
using IdentityModel.Client;
using LifestyleChecker.Contracts.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit.Gherkin.Quick;

namespace LifestyleChecker.Tests
{
    [FeatureFile("./Features/PatientIdentification.feature")]
    public sealed class PatientIdentificationTests : Feature
    {
        private string _nhsNumber;
        private string _surname;
        private DateTime _dateOfBirth;
        private HttpResponseMessage _response;

        [Given(@"the patient has an NHS number ""(\w+)""")]
        public void GivenThePatientHasAnNHSNumber(string nhsNumber)
        {
            _nhsNumber = nhsNumber;
        }

        [And(@"the patient has a surname ""(\w+)""")]
        public void GivenThePatientHasASurname(string surname)
        {
            _surname = surname;
        }

        [And(@"the patient has a date of birth ""(\d{2}-\d{2}-\d{4})""")]
        public void GivenThePatientHasADateOfBirth(DateTime dateOfBirth)
        {
            _dateOfBirth = dateOfBirth;
        }

        [When(@"the patient is identified")]
        public async Task WhenThePatientIsIdentified()
        {
            var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.LifestyleChecker_AppHost>();
            appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
            {
                clientBuilder.AddStandardResilienceHandler();
            });

            await using var app = await appHost.BuildAsync();
            var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
            await app.StartAsync();

            var httpClient = app.CreateHttpClient("apiservice");
            await resourceNotificationService.WaitForResourceAsync("apiservice", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
            var request = new HttpRequestMessage(HttpMethod.Get, "api/User/Me");
            request.Headers.Authorization = AuthenticationHeaderValue.Parse($"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_nhsNumber}|{_surname}:{_dateOfBirth}"))}");
            _response = await httpClient.SendAsync(request);
        }

        [Then(@"the identification should be successful")]
        public void ThenThePatientShouldBeSuccessfullyIdentified()
        {
            Assert.True(_response.IsSuccessStatusCode, "The patient identification should be successful.");
        }

        [Then(@"the identification should fail")]
        public void ThenThePatientIdentificationShouldFail()
        {
            Assert.False(_response.IsSuccessStatusCode);
        }

        [And(@"the system should return an error message (""[^""]*"")|('[^']*')")]
        public async Task AndTheSystemShouldReturnAnErrorMessage(string errorMessage)
        {
            // Remove any leading or trailing double quotes from the errorMessage
            errorMessage = errorMessage?.Trim('"');
            Assert.Equal(errorMessage, await _response.Content.ReadAsStringAsync());
        }

        [And(@"the system should return ""(\w+)"", ""(\w+)"", ""(\w+)"", ""(\d{2}-\d{2}-\d{4})""")]
        public async Task AndTheSystemShouldReturnThePatientDetails(string nhsNumber, string forename, string surname, DateTime dateOfBirth)
        {
            var patientDetails = await _response.Content.ReadFromJsonAsync<PatientDTO>();
            Assert.NotNull(patientDetails);
            Assert.Equal(nhsNumber, patientDetails.NHSNumber);
            Assert.Equal(forename, patientDetails.Forename, ignoreCase: true);
            Assert.Equal(surname, patientDetails.Surname, ignoreCase: true);
            Assert.Equal(dateOfBirth, patientDetails.DateOfBirth);
        }
    }
}
