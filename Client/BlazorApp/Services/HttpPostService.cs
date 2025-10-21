using System.Text.Json;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient httpClient;
    
    public HttpPostService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    public Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        HttpResponseMessage httpResponse = await httpClient.PostAsJsonAsync("posts", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
    
    public Task UpdatePostAsync(int id, PostUpdateDto request)
    {
        HttpResponseMessage httpResponse = await httpClient.PutAsJsonAsync($"posts/{id}", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
    
    public Task DeletePostAsync(int id)
    {
        HttpResponseMessage httpResponse = await httpClient.DeleteAsync($"posts/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return Task.CompletedTask;
    }
    
    public Task<PostDto> GetPostAsync(int id)
    {
        HttpResponseMessage httpResponse = await httpClient.GetAsync($"posts/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
    
    public Task<List<PostDto>> GetPostsAsync(string? search = null, int page = 1, int pageSize = 50)
    {
        HttpResponseMessage httpResponse = await httpClient.GetAsync($"posts?search={search}&page={page}&pageSize={pageSize}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<List<PostDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
    
    
}