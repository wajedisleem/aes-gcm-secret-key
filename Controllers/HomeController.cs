using Microsoft.AspNetCore.Mvc;

namespace AESGCMSecretKey.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
  [HttpGet]
  public IActionResult Index()
  {
    return Ok("Welcome to the AES-GCM Secret Key Generator API!");
  }
}