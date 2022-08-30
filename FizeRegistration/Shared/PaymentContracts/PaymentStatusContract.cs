using System.Text.Json.Serialization;

namespace FizeRegistration.Shared.PaymentContracts;

public class PaymentStatusContract
{
    [JsonPropertyName("version")]
    public int Version { get; set; }


    [JsonPropertyName("public_key")]
    public string PublicKey { get; set; }


    [JsonPropertyName("action")]
    public string Action { get; set; }


    [JsonPropertyName("order_id")]
    public string OrderId { get; set; }
}