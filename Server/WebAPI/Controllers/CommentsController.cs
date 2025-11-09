using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryContract;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;

    public CommentsController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

 
    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create([FromBody] CommentCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Body))
            return BadRequest("Body is required.");

        var created = await _commentRepository.AddAsync(new Comment
        {
            PostId = dto.PostId,
            UserId = dto.UserId,
            Body = dto.Body,
           
        });

        var result = new CommentDto(created.Id, created.PostId, created.UserId, created.Body);
        return CreatedAtAction(nameof(GetSingle), new { id = created.Id }, result);
    }

  
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetSingle(int id)
    {
        try
        {
            var c = await _commentRepository.GetSingleAsync(id);
            return Ok(new CommentDto(c.Id, c.PostId, c.UserId, c.Body));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

   
    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetMany([FromQuery] int? postId)
    {
        var q = _commentRepository.GetMany();
        if (postId.HasValue) q = q.Where(c => c.PostId == postId.Value);

        var list = q.Select(c => new CommentDto(c.Id, c.PostId, c.UserId, c.Body)).ToList();
        return Ok(list);
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CommentUpdateDto dto)
    {
        if (id != dto.Id) return BadRequest("ID mismatch.");

        try
        {
            var existing = await _commentRepository.GetSingleAsync(id);
            existing.Body = dto.Body;
            await _commentRepository.UpdateAsync(existing);
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
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}
