using System.Text.Json.Serialization;

namespace FizeRegistration.Shared.DataContracts;


public class TokenDataContract
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
}