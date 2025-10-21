using ApiContracts;

namespace BlazorApp.Services;

public interface IUserService
{
    public Task<UserDto> AddUserAsync(UserCreateDto request);
    public Task UserUpdateAsync(int id, UserUpdateDto request);
    public Task<UserDto?> DeleteUserAsync(int id);
    public Task<UserDto> GetUserAsync(int id);
    public Task<List<UserDto>> GetUsersAsync(string? search = null, int page = 1, int pageSize = 50);
}