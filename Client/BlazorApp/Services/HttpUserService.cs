using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpUserService : IUserService
{
    private readonly HttpClient httpClient;

    public HttpUserService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<UserDto> AddUserAsync(UserCreateDto request)
    {
        var url = "api/users";
        Console.WriteLine($"➡️ POST {httpClient.BaseAddress}{url}");

        var httpResponse = await httpClient.PostAsJsonAsync(url, request);
        var response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception($"API returned {httpResponse.StatusCode}: {response}");

        return JsonSerializer.Deserialize<UserDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task UserUpdateAsync(int id, UserUpdateDto request)
    {
        var url = $"api/users/{id}";
        HttpResponseMessage httpResponse = await httpClient.PutAsJsonAsync(url, request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            Console.WriteLine($" Error updating user: {httpResponse.StatusCode}");
            throw new Exception(response);
        }
    }

    public async Task<UserDto?> DeleteUserAsync(int id)
    {
        var url = $"api/users/{id}";
        HttpResponseMessage httpResponse = await httpClient.DeleteAsync(url);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            Console.WriteLine($" Error deleting user: {httpResponse.StatusCode}");
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<UserDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<UserDto> GetUserAsync(int id)
    {
        var url = $"api/users/{id}";
        HttpResponseMessage httpResponse = await httpClient.GetAsync(url);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            Console.WriteLine($" Error fetching user {id}: {httpResponse.StatusCode}");
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<UserDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<List<UserDto>> GetUsersAsync(string? search = null, int page = 1, int pageSize = 50)
    {
        var url = $"api/users?search={search}&page={page}&pageSize={pageSize}";
        Console.WriteLine($" GET {httpClient.BaseAddress}{url}");

        HttpResponseMessage httpResponse = await httpClient.GetAsync(url);
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            Console.WriteLine($" API Error {httpResponse.StatusCode}: {response}");
            throw new Exception($"API returned {httpResponse.StatusCode}: {response}");
        }

        return JsonSerializer.Deserialize<List<UserDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}
