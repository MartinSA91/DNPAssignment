using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContract;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public UsersController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create(UserCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.UserName) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest("Username and password are required.");

        var exists = userRepository.GetMany().Any(u => u.UserName == dto.UserName);
        if (exists) return Conflict("Username already taken.");

        var user = new User { UserName = dto.UserName, Password = dto.Password };
        var created = await userRepository.AddAsync(user);

        return CreatedAtAction(nameof(GetSingle), new { id = created.Id },
            new UserDto(created.Id, created.UserName));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetSingle(int id)
    {
        try
        {
            var user = await userRepository.GetSingleAsync(id);
            return Ok(new UserDto(user.Id, user.UserName));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    public IEnumerable<UserDto> GetMany([FromQuery] string? search = null)
    {
        var q = userRepository.GetMany();
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(u => u.UserName.Contains(search, StringComparison.OrdinalIgnoreCase));

        return q.Select(u => new UserDto(u.Id, u.UserName));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UserUpdateDto dto)
    {
        if (id != dto.Id) return BadRequest("ID mismatch.");

        try
        {
            var user = await userRepository.GetSingleAsync(id);
            user.UserName = dto.UserName;
            user.Password = dto.Password;
            await userRepository.UpdateAsync(user);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await userRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}
