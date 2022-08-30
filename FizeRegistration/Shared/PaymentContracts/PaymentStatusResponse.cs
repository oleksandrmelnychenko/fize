using System.Text.Json.Serialization;

namespace FizeRegistration.Shared.PaymentContracts;

public class PaymentStatusResponse
{
    [JsonPropertyName("acq_id")]
    public int AcqId { get; set; }


    [JsonPropertyName("action")]
    public string Action { get; set; }


    [JsonPropertyName("result")]
    public string Result { get; set; }


    [JsonPropertyName("payment_id")]
    public int PaymentId { get; set; }


    [JsonPropertyName("status")]
    public string Status { get; set; }


    [JsonPropertyName("type")]
    public string Type { get; set; }


    [JsonPropertyName("paytype")]
    public string Paytype { get; set; }


    [JsonPropertyName("public_key")]
    public string PublicKey { get; set; }


    [JsonPropertyName("order_id")]
    public string OrderId { get; set; }


    [JsonPropertyName("liqpay_order_id")]
    public string LiqpayOrderId { get; set; }


    [JsonPropertyName("description")]
    public string Description { get; set; }


    [JsonPropertyName("sender_card_mask2")]
    public string SenderCardMask2 { get; set; }


    [JsonPropertyName("sender_card_bank")]
    public string SenderCardBank { get; set; }


    [JsonPropertyName("sender_card_type")]
    public string SenderCardType { get; set; }


    [JsonPropertyName("sender_card_country")]
    public int SenderCardCountry { get; set; }


    [JsonPropertyName("amount")]
    public double Amount { get; set; }


    [JsonPropertyName("ip")]
    public string Ip { get; set; }


    [JsonPropertyName("sender_commission")]
    public double SenderCommission { get; set; }


    [JsonPropertyName("receiver_commission")]
    public double ReceiverCommission { get; set; }


    [JsonPropertyName("agent_commission")]
    public double AgentCommission { get; set; }


    [JsonPropertyName("amount_debit")]
    public double AmountDebit { get; set; }


    [JsonPropertyName("amount_credit")]
    public double AmountCredit { get; set; }


    [JsonPropertyName("commission_debit")]
    public double CommissionDebit { get; set; }


    [JsonPropertyName("commission_credit")]
    public double CommissionCredit { get; set; }


    [JsonPropertyName("currency_debit")]
    public string CurrencyDebit { get; set; }


    [JsonPropertyName("currency_credit")]
    public string CurrencyCredit { get; set; }


    [JsonPropertyName("sender_bonus")]
    public double SenderBonus { get; set; }


    [JsonPropertyName("amount_bonus")]
    public double AmountBonus { get; set; }


    [JsonPropertyName("mpi_eci")]
    public byte MpiEci { get; set; }


    [JsonPropertyName("is_3ds")]
    public bool Is3ds { get; set; }


    [JsonPropertyName("language")]
    public string Language { get; set; }


    [JsonPropertyName("create_date")]
    public long CreateDate { get; set; }


    [JsonPropertyName("end_date")]
    public long EndDate { get; set; }


    [JsonPropertyName("transaction_id")]
    public int TransactionId { get; set; }
}