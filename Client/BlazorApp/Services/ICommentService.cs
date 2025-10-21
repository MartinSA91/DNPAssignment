using ApiContracts;

namespace BlazorApp.Services;

public interface ICommentService
{
    public Task<CommentDto> AddCommentAsync(CommentCreateDto request);
    public Task UpdateCommentAsync(int id, CommentUpdateDto request);
    public Task DeleteCommentAsync(int id);
    public Task<CommentDto> GetCommentAsync(int id);
    public Task<List<CommentDto>> GetCommentsAsync(string? search = null, int page =
    1, int pageSize = 50);
    
}