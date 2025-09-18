using System.Text.Json;
using Entities;
using RepositoryContract;

namespace FileRepositories
{

    public class CommentFileRepository : ICommentRepository
    {
        private readonly string filePath = "comments.json";

        public CommentFileRepository()
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]");
            }
        }

        private async Task<List<Comment>> LoadAsync()
        {
            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<Comment>>(json) ??
                new List<Comment>();
        }

        private async Task SaveAsync(List<Comment> comments)
        {
            var json = JsonSerializer.Serialize(comments);
            await File.WriteAllTextAsync(filePath, json);
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            var comments = await LoadAsync();
            comment.Id = comments.Any() ? comments.Max(c => c.Id) + 1 : 1;
            comments.Add(comment);
            await SaveAsync(comments);
            return comment;
        }

        public async Task UpdateAsync(Comment comment)
        {
            var comments = await LoadAsync();
            var existing = comments.SingleOrDefault(c => c.Id == comment.Id);
            if (existing is null)
            {
                throw new InvalidOperationException(
                    $"Comment with ID {comment.Id} not found");
            }

            comments.Remove(existing);
            comments.Add(comment);
            await SaveAsync(comments);
        }

        public async Task DeleteAsync(int id)
        {
            var comments = await LoadAsync();
            var existing = comments.SingleOrDefault(c => c.Id == id);
            if (existing is null)
            {
                throw new InvalidOperationException(
                    $"Comment with ID {id} not found");
            }

            comments.Remove(existing);
            await SaveAsync(comments);
        }
        
        public async Task<Comment> GetSingleAsync(int id)
        {
            var comments = await LoadAsync();
            var comment = comments.SingleOrDefault(c => c.Id == id);
            if (comment is null)
            {
                throw new InvalidOperationException(
                    $"Comment with ID {id} not found");
            }

            return comment;
        }

        public IQueryable<Comment> GetMany()
        {
            var comments = File.ReadAllText(filePath);
            var list = JsonSerializer.Deserialize<List<Comment>>(comments) ??
                       new List<Comment>();
            return list.AsQueryable();

        }
        
    }
}
