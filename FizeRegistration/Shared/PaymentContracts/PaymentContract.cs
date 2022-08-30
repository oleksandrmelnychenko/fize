using System.Text.Json.Serialization;

namespace FizeRegistration.Shared.PaymentContracts;


public class PaymentContract
{
    [JsonPropertyName("version")]
    public int Version { get; set; }


    [JsonPropertyName("public_key")]
    public string PublicKey { get; set; }


    [JsonPropertyName("action")]
    public string Action { get; set; }


    [JsonPropertyName("amount")]
    public double Amount { get; set; }


    [JsonPropertyName("currency")]
    public string Currency { get; set; }


    [JsonPropertyName("description")]
    public string Description { get; set; }


    [JsonPropertyName("order_id")]
    public string OrderId { get; set; }


    [JsonPropertyName("result_url")]
    public string ResultUrl { get; set; }
}