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
        if (string.IsNullOrWhiteSpace(dto.UserName)) return BadRequest("Username required.");

      
        var exists = userRepository.GetMany().Any(u => u.UserName == dto.UserName);
        if (exists) return Conflict("Username already taken.");

        var user = await userRepository.AddAsync(new User { UserName = dto.UserName });
        return CreatedAtAction(nameof(GetSingle), new { id = user.Id }, new UserDto(user.Id, user.UserName));
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
    public ActionResult<IEnumerable<UserDto>> GetMany([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var query = userRepository.GetMany();
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(u => u.UserName.Contains(search));

        var items = query
            .OrderBy(u => u.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserDto(u.Id, u.UserName))
            .ToList();

        return Ok(items);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UserUpdateDto dto)
    {
        if (id != dto.Id) return BadRequest("ID mismatch.");

        try
        {
            await userRepository.UpdateAsync(new User { Id = dto.Id, UserName = dto.UserName });
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
