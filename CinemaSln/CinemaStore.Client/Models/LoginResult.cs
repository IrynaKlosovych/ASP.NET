using System.Text.Json.Serialization;

namespace CinemaStore.Client.Models
{
    public class LoginResult
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
