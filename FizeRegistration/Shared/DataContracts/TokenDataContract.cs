namespace FizeRegistration.Shared.DataContracts;


public class TokenDataContract
{
    public string Token { get; set; }

    public DateTime ExpiresAt { get; set; }
}