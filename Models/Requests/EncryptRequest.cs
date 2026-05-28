using System.ComponentModel.DataAnnotations;

namespace AESGCMSecretKey.Models.Requests;

public sealed record EncryptRequest(
    [property: Required(ErrorMessage = "plainText is required.")]
    [property: RegularExpression(@".*\S.*", ErrorMessage = "plainText cannot be empty or whitespace.")]
    string PlainText
);
