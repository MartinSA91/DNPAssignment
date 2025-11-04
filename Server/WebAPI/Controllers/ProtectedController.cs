using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ProtectedController
{
    [HttpGet("secure-data")]
    [Authorize]

    public IActionResult SecureData()
    {
        return new OkObjectResult("This is protected data accessible only to authenticated users.");
    }
}