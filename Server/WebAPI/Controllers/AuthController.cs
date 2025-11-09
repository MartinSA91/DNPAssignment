using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiContracts;
using Entities;
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
    public IActionResult Login([FromBody] LoginRequestDto dto)
    {
        // Tjek mod dit repository i stedet for hardcoded
        var user = userRepository.GetMany()
            .FirstOrDefault(u => u.UserName == dto.UserName && u.Password == dto.Password);

        if (user is null)
            return Unauthorized("Invalid username or password");

        // Token creation
        var jwtSection = configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
       

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        

        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSection["ExpiresInMinutes"])),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new LoginResponseDto(tokenString, user.Id, user.UserName)
       );
    }
    
}
