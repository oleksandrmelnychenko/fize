using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FizeRegistration.Services.PaymentServices.Contracts;
using FizeRegistration.Shared.DataContracts;
using FizeRegistration.Shared.PaymentContracts;

namespace FizeRegistration.Services.PaymentServices;

public class LiqPayService : ILiqPayService
{
    private string _private_key = /*privateApiKey*/ string.Empty;

    private string _public_key = /*publicApiKey*/ string.Empty;

    public LiqPayService()
    {
        
    }

    public async Task GetStatus(PaymentStatusContract paymentStatusContract)
    {
        using (var client = new HttpClient())
        {
            var liqPayRequest = LiqPayDataJsonToLiqPayRequest(System.Text.Json.JsonSerializer.Serialize(paymentStatusContract));

            var dict = new Dictionary<string, string>
            {
            {"data", liqPayRequest.Data},
            {"signature", liqPayRequest.Signature}
            };

            using (var content = new FormUrlEncodedContent(dict))
            {
                content.Headers.Clear();
                
                content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                HttpResponseMessage response = await client.PostAsync("https://www.liqpay.ua/api/request", content);

                var b = await response.Content.ReadFromJsonAsync<PaymentStatusResponse>();

                Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(b));

                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
        }
    }

    public LiqPayRequest LiqPayDataJsonToLiqPayRequest(string liqPayDataJsonString)
    {
        var data = Base64Encode(liqPayDataJsonString);

        var unhashedSignature = _private_key + data + _private_key;

        using var sha1 = SHA1.Create();

        var hexHashedSignature = Convert.ToHexString(sha1.ComputeHash(Encoding.UTF8.GetBytes(unhashedSignature)));

        var signature = HexString2B64String(hexHashedSignature);

        return new LiqPayRequest
        {
            Data = data,
            Signature = signature
        };
    }

    public static byte[] HexStringToHex(string inputHex)
    {
        var resultantArray = new byte[inputHex.Length / 2];

        for (var i = 0; i < resultantArray.Length; i++)
        {
            resultantArray[i] = System.Convert.ToByte(inputHex.Substring(i * 2, 2), 16);
        }

        return resultantArray;
    }

    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);

        return System.Convert.ToBase64String(plainTextBytes);
    }

    public static string HexString2B64String(string input)
    {
        return System.Convert.ToBase64String(HexStringToHex(input));
    }
}