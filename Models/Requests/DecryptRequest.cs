using System.ComponentModel.DataAnnotations;

namespace AESGCMSecretKey.Models.Requests;

public sealed record DecryptRequest(
    [property: Required(ErrorMessage = "encryptedText is required.")]
    [property: RegularExpression(@"^[A-Za-z0-9_-]+$", ErrorMessage = "encryptedText must be Base64Url.")]
    string EncryptedText
);
