using System.Text.Json;
using System.Text.Json.Serialization;

namespace LifestyleChecker.SharedUtilities.Converters
{
    /// <summary>
    /// Provides custom JSON serialization and deserialization for <see cref="DateTime"/> objects using the "dd-MM-yyyy"
    /// format.
    /// </summary>
    /// <remarks>This converter ensures that <see cref="DateTime"/> values are serialized to and deserialized
    /// from JSON strings in the "dd-MM-yyyy" format. It is useful when working with JSON data that requires a specific
    /// date format.</remarks>
    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        /// <summary>
        /// Reads a JSON string and converts it to a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="reader">The <see cref="Utf8JsonReader"/> to read from.</param>
        /// <param name="typeToConvert">The type of the object to convert, which is ignored in this implementation.</param>
        /// <param name="options">The serializer options to use, which are ignored in this implementation.</param>
        /// <returns>A <see cref="DateTime"/> object parsed from the JSON string.</returns>
        /// <exception cref="JsonException">Thrown if the JSON string is null or cannot be parsed as a date in the format "dd-MM-yyyy".</exception>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string dateString = reader.GetString();
            if (dateString == null)
            {
                throw new JsonException("Date string is null.");
            }
            return DateTime.ParseExact(dateString, "dd-MM-yyyy", null);
        }

        /// <summary>
        /// Writes a <see cref="DateTime"/> value as a JSON string in the format "dd-MM-yyyy".
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which the JSON string is written. Cannot be null.</param>
        /// <param name="value">The <see cref="DateTime"/> value to write.</param>
        /// <param name="options">The serialization options to use. This parameter is not used in this method.</param>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("dd-MM-yyyy"));
        }
    }
}
