using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContract;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepository;

    public CommentsController(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetSingle(int id)
    {
        try
        {
            var c = await commentRepository.GetSingleAsync(id);
            return Ok(new CommentDto(c.Id, c.PostId, c.UserId, c.Content));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    // GET api/comments?postId=10
    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetMany([FromQuery] int? postId)
    {
        var q = commentRepository.GetMany();
        if (postId.HasValue) q = q.Where(c => c.PostId == postId.Value);
        var list = q.Select(c => new CommentDto(c.Id, c.PostId, c.UserId, c.Content)).ToList();
        return Ok(list);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CommentUpdateDto dto)
    {
        if (id != dto.Id) return BadRequest("ID mismatch.");

        try
        {
            var existing = await commentRepository.GetSingleAsync(id);
            existing.Content = dto.Content;
            await commentRepository.UpdateAsync(existing);
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
            await commentRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}