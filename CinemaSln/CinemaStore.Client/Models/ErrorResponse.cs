using System.Text.Json.Serialization;

namespace CinemaStore.Client.Models
{
    public class ErrorResponse
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}
