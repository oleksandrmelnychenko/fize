using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using FizeRegistration.Shared.DataContracts;
using Microsoft.AspNetCore.Components.Authorization;

namespace FizeRegistration.Client.Helpers;
public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;

    public AuthStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        TokenDataContract? tokenResponse = null;
        try
        {
            tokenResponse = await _localStorage.GetItemAsync<TokenDataContract>("token");
        }
        catch
        {

        }

        var identity = new ClaimsIdentity();

        if (tokenResponse != null)
        {
            try
            {
                if (tokenResponse.ExpiresAt > DateTime.UtcNow)
                {
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(tokenResponse.Token), "jwt");
                }
            }
            catch
            {

            }
        }

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        if (keyValuePairs == null)
        {
            return new List<Claim>();
        }

        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? String.Empty));
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}
