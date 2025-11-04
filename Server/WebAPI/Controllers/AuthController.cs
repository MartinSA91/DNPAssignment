using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RepositoryContract;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration configuration;
    private readonly IUserRepository userRepository;

    public AuthController(IConfiguration configuration, IUserRepository userRepository)
    {
        this.configuration = configuration;
        this.userRepository = userRepository;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        // Tjek mod dit repository i stedet for hardcoded
        var user = userRepository.GetMany()
            .FirstOrDefault(u => u.UserName == model.UserName && u.Password == model.Password);

        if (user is null)
            return Unauthorized("Invalid username or password");

        // Token creation
        var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
        var issuer = configuration["Jwt:Issuer"];
        var audience = configuration["Jwt:Audience"];

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        var signingKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new
        {
            token = tokenString,
            user.Id,
            user.UserName
        });
    }

    public class LoginModel
    {
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
