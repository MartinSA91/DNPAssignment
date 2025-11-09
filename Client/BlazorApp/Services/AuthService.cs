using System.Text.Json;
using System.Net.Http.Json;
using ApiContracts;
using Microsoft.JSInterop;

namespace BlazorApp.Services;

public class AuthService
{
    private readonly HttpClient httpClient;
    private readonly IJSRuntime jsRuntime;

    public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        this.httpClient = httpClient;
        this.jsRuntime = jsRuntime;
    }

    public async Task<LoginResponseDto?> LoginAsync(string username, string password)
    {
        var response = await httpClient.PostAsJsonAsync("api/auth/login", new LoginRequestDto(username, password));
        
        var content = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Login failed: {content}");
        
        var loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(content, new  JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", "jwt", loginResponse.Token);
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResponse.Token);
        return loginResponse;
    }
}