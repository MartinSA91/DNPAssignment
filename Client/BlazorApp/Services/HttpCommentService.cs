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
        var httpResponse = await httpClient.PostAsJsonAsync("comments", request);
        var response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);

        return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task UpdateCommentAsync(int id, CommentUpdateDto request)
    {
        var httpResponse = await httpClient.PutAsJsonAsync($"comments/{id}", request);
        var response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);
    }

    public async Task DeleteCommentAsync(int id)
    {
        var httpResponse = await httpClient.DeleteAsync($"comments/{id}");
        var response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);
    }

    public async Task<CommentDto> GetCommentAsync(int id)
    {
        var httpResponse = await httpClient.GetAsync($"comments/{id}");
        var response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);

        return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<List<CommentDto>> GetCommentsAsync(string? search = null, int page = 1, int pageSize = 50)
    {
        var httpResponse = await httpClient.GetAsync($"comments?search={search}&page={page}&pageSize={pageSize}");
        var response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);

        return JsonSerializer.Deserialize<List<CommentDto>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}