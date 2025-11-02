using System.Text.Json;
using System.Net.Http.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient httpClient;

    public HttpCommentService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<CommentDto> AddCommentAsync(CommentCreateDto request)
    {
        var url = "api/comments";
        Console.WriteLine($"➡️ POST {httpClient.BaseAddress}{url}");

        var httpResponse = await httpClient.PostAsJsonAsync(url, request);
        var response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            Console.WriteLine($" Error creating comment: {httpResponse.StatusCode}");
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task UpdateCommentAsync(int id, CommentUpdateDto request)
    {
        var url = $"api/comments/{id}";
        var httpResponse = await httpClient.PutAsJsonAsync(url, request);
        var response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            Console.WriteLine($" Error updating comment: {httpResponse.StatusCode}");
            throw new Exception(response);
        }
    }

    public async Task DeleteCommentAsync(int id)
    {
        var url = $"api/comments/{id}";
        var httpResponse = await httpClient.DeleteAsync(url);
        var response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            Console.WriteLine($" Error deleting comment: {httpResponse.StatusCode}");
            throw new Exception(response);
        }
    }

    public async Task<CommentDto> GetCommentAsync(int id)
    {
        var url = $"api/comments/{id}";
        var httpResponse = await httpClient.GetAsync(url);
        var response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            Console.WriteLine($" Error fetching comment: {httpResponse.StatusCode}");
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<List<CommentDto>> GetCommentsAsync(string? search = null, int page = 1, int pageSize = 50)
    {
        var url = $"api/comments?search={search}&page={page}&pageSize={pageSize}";
        Console.WriteLine($" GET {httpClient.BaseAddress}{url}");

        var httpResponse = await httpClient.GetAsync(url);
        var response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
        {
            Console.WriteLine($" API Error {httpResponse.StatusCode}: {response}");
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<List<CommentDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}
