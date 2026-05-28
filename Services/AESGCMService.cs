using System.Text;
using System.Security.Cryptography;

namespace AESGCMSecretKey.Services;

public class AESGCMService
{
    private readonly byte[] masterKey;

    public AESGCMService(IConfiguration configuration)
    {
        var key = configuration["Security:MasterKey"];
        if (string.IsNullOrWhiteSpace(key))
            throw new Exception("MasterKey is missing.");

        masterKey = Convert.FromBase64String(key);

        if (masterKey.Length != 32)
            throw new Exception("MasterKey must be 32 bytes for AES-256-GCM.");
    }

    public string Encrypt(string plainText)
    {
        byte[] nonce = RandomNumberGenerator.GetBytes(12);
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] cipherBytes = new byte[plainBytes.Length];
        byte[] tag = new byte[16];

        using var aes = new AesGcm(masterKey, 16);
        aes.Encrypt(nonce, plainBytes, cipherBytes, tag);
        byte[] result = new byte[nonce.Length + cipherBytes.Length + tag.Length];

        Buffer.BlockCopy(nonce, 0, result, 0, nonce.Length);
        Buffer.BlockCopy(cipherBytes, 0, result, nonce.Length, cipherBytes.Length);
        Buffer.BlockCopy(tag, 0, result, nonce.Length + cipherBytes.Length, tag.Length);

        return Base64UrlEncode(result);
    }

    public string Decrypt(string encryptedText)
    {
        byte[] fullData = Base64UrlDecode(encryptedText);
        byte[] nonce = fullData[..12];
        byte[] tag = fullData[^16..];
        byte[] cipherBytes = fullData[12..^16];
        byte[] plainBytes = new byte[cipherBytes.Length];

        using var aes = new AesGcm(masterKey, 16);
        aes.Decrypt(nonce, cipherBytes, tag, plainBytes);

        return Encoding.UTF8.GetString(plainBytes);
    }

    private static string Base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
    }

    private static byte[] Base64UrlDecode(string input)
    {
        string padded = input
            .Replace("-", "+")
            .Replace("_", "/");

        switch (padded.Length % 4)
        {
            case 2:
                padded += "==";
                break;

            case 3:
                padded += "=";
                break;
        }

        return Convert.FromBase64String(padded);
    }
}