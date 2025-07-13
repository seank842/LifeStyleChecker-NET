using LifestyleChecker.Contracts.DTOs;

namespace LifestyleChecker.Services.PatientService
{
    public interface IPatientLookup
    {
        Task<PatientDTO> GetPatientByIdAsync(string nHSNumber);
    }
}
