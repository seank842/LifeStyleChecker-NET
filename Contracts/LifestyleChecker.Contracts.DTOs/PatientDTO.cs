using LifestyleChecker.SharedUtilities.Converters;
using System.Text.Json.Serialization;

namespace LifestyleChecker.Contracts.DTOs
{
    /// <summary>
    /// Represents a data transfer object for patient information.
    /// </summary>
    /// <remarks>This class is used to transfer patient data, including NHS number, name, and date of birth,
    /// between different layers of an application. It is typically used in scenarios where patient information needs to
    /// be serialized or deserialized, such as in web API communications.</remarks>
    public class PatientDTO
    {
        [JsonPropertyName("nhsNumber")]
        public required string NHSNumber { get; set; }
        [JsonPropertyName("name")]
        public required string Name { get; set; }
        public string Forename => (Name.Split(',').LastOrDefault() ?? string.Empty).Trim();
        public string Surname => (Name.Split(',').FirstOrDefault() ?? string.Empty).Trim();
        [JsonPropertyName("born")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public required DateTime DateOfBirth { get; set; }
    }
}
