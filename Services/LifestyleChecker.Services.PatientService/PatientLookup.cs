using LifestyleChecker.Contracts.DTOs;
using System.Net.Http.Json;

namespace LifestyleChecker.Services.PatientService
{
    public class PatientLookup : IPatientLookup
    {
        private readonly HttpClient _httpClient;

        public PatientLookup(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PatientDTO> GetPatientByIdAsync(string nHSNumber)
        {
            var response = await _httpClient.GetAsync($"/tech-test/t2/patients/{nHSNumber}");
            if (response.IsSuccessStatusCode)
            {
                var patient = await response.Content.ReadFromJsonAsync<PatientDTO>();
                return patient ?? throw new Exception($"Patient data is not formatted correctly from Lookup service");
            }
            else
            {
                throw new Exception($"Error retrieving patient with ID {nHSNumber}: {response.ReasonPhrase}");
            }
        }
    }
}
