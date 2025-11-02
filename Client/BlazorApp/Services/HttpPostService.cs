using System.Text.Json;
using System.Net.Http.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient httpClient;

    public HttpPostService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<PostDto> AddPostAsync(PostCreateDto request)
    {
        var url = "api/posts";
        Console.WriteLine($"️ POST {httpClient.BaseAddress}{url}");

        var httpResponse = await httpClient.PostAsJsonAsync(url, request);
        var response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            Console.WriteLine($" Error creating post: {httpResponse.StatusCode}");
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<PostDto> UpdatePostAsync(int id, PostUpdateDto request)
    {
        var url = $"api/posts/{id}";
        var httpResponse = await httpClient.PutAsJsonAsync(url, request);
        var response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            Console.WriteLine($" Error updating post {id}: {httpResponse.StatusCode}");
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<PostDto> DeletePostAsync(int id)
    {
        var url = $"api/posts/{id}";
        var httpResponse = await httpClient.DeleteAsync(url);
        var response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            Console.WriteLine($" Error deleting post {id}: {httpResponse.StatusCode}");
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<PostDto> GetPostAsync(int id)
    {
        var url = $"api/posts/{id}";
        var httpResponse = await httpClient.GetAsync(url);
        var response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            Console.WriteLine($" Error fetching post {id}: {httpResponse.StatusCode}");
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<List<PostDto>> GetPostsAsync(string? search = null, int page = 1, int pageSize = 50)
    {
        var url = $"api/posts?search={search}&page={page}&pageSize={pageSize}";
        

        var httpResponse = await httpClient.GetAsync(url);
        var response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            Console.WriteLine($" API Error {httpResponse.StatusCode}: {response}");
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<List<PostDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}
