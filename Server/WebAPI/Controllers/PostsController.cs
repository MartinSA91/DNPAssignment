using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContract;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;

    public PostsController(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> Create(PostCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title)) return BadRequest("Title required.");
        var post = await postRepository.AddAsync(new Post
        {
            Title = dto.Title,
            Body = dto.Body,
            UserId = dto.UserId,
        });

        var result = new PostDto(post.Id, post.Title, post.Body, post.UserId, null);
        return CreatedAtAction(nameof(GetSingle), new { id = post.Id }, result);
    }

  
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetSingle(int id, [FromQuery] bool includeComments = false)
    {
        try
        {
            var post = await postRepository.GetSingleAsync(id);

            IEnumerable<CommentDto>? commentsDto = null;
            if (includeComments)
            {
                var comments = commentRepository.GetMany().Where(c => c.PostId == id);
                commentsDto = comments.Select(c => new CommentDto(c.Id, c.PostId, c.UserId, c.Body));
            }

            var dto = new PostDto(post.Id, post.Title, post.Body, post.UserId, commentsDto);
            return Ok(dto);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    
    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> GetMany([FromQuery] int? userId, [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var query = postRepository.GetMany();

        if (userId.HasValue) query = query.Where(p => p.UserId == userId.Value);
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Title.Contains(search) || p.Body.Contains(search));

        var items = query
            .OrderByDescending(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PostDto(p.Id, p.Title, p.Body, p.UserId, null))
            .ToList();

        return Ok(items);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, PostUpdateDto dto)
    {
        if (id != dto.Id) return BadRequest("ID mismatch.");

        try
        {
            await postRepository.UpdateAsync(new Post
            {
                Id = dto.Id,
                Title = dto.Title,
                Body = dto.Body
            });
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await postRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    
    [HttpPost("{postId:int}/comments")]
    public async Task<ActionResult<CommentDto>> AddComment(int postId, CommentCreateDto dto, [FromServices] IUserRepository userRepository)
    {
        if (postId != dto.PostId) return BadRequest("PostId mismatch.");
        
        try { _ = await postRepository.GetSingleAsync(postId); } catch { return NotFound("Post not found."); }
        try { _ = await userRepository.GetSingleAsync(dto.UserId); } catch { return NotFound("User not found."); }

        var comment = await commentRepository.AddAsync(new Comment
        {
            PostId = dto.PostId,
            UserId = dto.UserId,
            Body = dto.Body,
            
        });

        var result = new CommentDto(comment.Id, comment.PostId, comment.UserId, comment.Body);
        return CreatedAtAction(nameof(GetPostComment), new { postId, commentId = comment.Id }, result);
    }


    [HttpGet("{postId:int}/comments/{commentId:int}")]
    public async Task<ActionResult<CommentDto>> GetPostComment(int postId, int commentId)
    {
        try
        {
            var comment = await commentRepository.GetSingleAsync(commentId);
            if (comment.PostId != postId) return NotFound();
            return Ok(new CommentDto(comment.Id, comment.PostId, comment.UserId, comment.Body));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}
