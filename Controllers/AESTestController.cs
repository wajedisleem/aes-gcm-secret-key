using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using AESGCMSecretKey.Services;
using AESGCMSecretKey.Models.Requests;

namespace AESGCMSecretKey.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AESTestController : ControllerBase
{
    private readonly AESGCMService aesgcmService;

    public AESTestController(AESGCMService aesgcmService)
    {
        this.aesgcmService = aesgcmService;
    }

    [HttpGet("generate-key")]
    public IActionResult GenerateMasterKey()
    {
        var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        return Ok(key);
    }

    [HttpPost("encrypt")]
    public IActionResult Encrypt(EncryptRequest request)
    {
        var encrypted = aesgcmService.Encrypt(request.PlainText);
        return Ok(encrypted);
    }

    [HttpPost("decrypt")]
    public IActionResult Decrypt(DecryptRequest request)
    {
        try
        {
            var plainText = aesgcmService.Decrypt(request.EncryptedText);
            return Ok(plainText);
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Invalid encryptedText or key mismatch." });
        }
    }
}
