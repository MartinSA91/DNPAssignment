namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient httpClient;
    
    public HttpCommentService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    public Task<CommentDto> AddCommentAsync(CreateCommentDto request)
    {
        throw new NotImplementedException();
    }
    
    public Task UpdateCommentAsync(int id, CommentUpdateDto request)
    {
        throw new NotImplementedException();
    }
    
    public Task DeleteCommentAsync(int id)
    {
        throw new NotImplementedException();
    }
    
    public Task<CommentDto> GetCommentAsync(int id)
    {
        throw new NotImplementedException();
    }
    
    public Task<List<CommentDto>> GetCommentsAsync(string? search = null, int page = 1, int pageSize = 50)
    {
        throw new NotImplementedException();
    }
    
    
}