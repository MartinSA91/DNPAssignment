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
        HttpResponseMessage httpResponse = await httpClient.PostAsJsonAsync("users", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<UserDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
    
    public async Task UserUpdateAsync(int id, UserUpdateDto request)
    {
        HttpResponseMessage httpResponse = await httpClient.PutAsJsonAsync($"users/{id}", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)  
        {
            throw new Exception(response);
        }
    }
    
    public async Task<UserDto?> DeleteUserAsync(int id)
    {
        HttpResponseMessage httpResponse = await httpClient.DeleteAsync($"users/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)  
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<UserDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
    
    public async Task<UserDto> GetUserAsync(int id)
    {
        HttpResponseMessage httpResponse = await httpClient.GetAsync($"users/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<UserDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
    
    public async Task<List<UserDto>> GetUsersAsync(string? search = null, int page = 1, int pageSize = 50)
    {
        HttpResponseMessage httpResponse = await httpClient.GetAsync($"users?search={search}&page={page}&pageSize={pageSize}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<List<UserDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}