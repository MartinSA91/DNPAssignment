using System.Text.Json;
using Entities;
using RepositoryContract;

namespace FileRepositories
{
    public class PostFileRepository : IPostRepository
    {
        private readonly string filePath = "posts.json";
        
        public PostFileRepository()
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]");
            }
        }
        
        private async Task<List<Post>> LoadAsync()
        {
            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<Post>>(json) ?? new List<Post>();
        }
        
        private async Task SaveAsync(List<Post> posts)
        {
            var json = JsonSerializer.Serialize(posts);
            await File.WriteAllTextAsync(filePath, json);
        }
        
        public async Task<Post> AddAsync(Post post)
        {
            var posts = await LoadAsync();
            post.Id = posts.Any() ? posts.Max(p => p.Id) + 1 : 1;
            posts.Add(post);
            await SaveAsync(posts);
            return post;
        }
        
        public async Task UpdateAsync(Post post)
        {
            var posts = await LoadAsync();
            var existing = posts.SingleOrDefault(p => p.Id == post.Id);
            if (existing is null)
            {
                throw new InvalidOperationException($"Post with ID {post.Id} not found");
            }
            
            posts.Remove(existing);
            posts.Add(post);
            await SaveAsync(posts);
        }
        
        public async Task DeleteAsync(int id)
        {
            var posts = await LoadAsync();
            var existing = posts.SingleOrDefault(p => p.Id == id);
            if (existing is null)
            {
                throw new InvalidOperationException($"Post with ID {id} not found");
            }
            
            posts.Remove(existing);
            await SaveAsync(posts);
        }

        public async Task<Post> GetSingleAsync(int id)
        {
            var posts = await LoadAsync();
            var post = posts.SingleOrDefault(p => p.Id == id);
            if (post is null)
            {
                throw new InvalidOperationException($"Post with ID {id} not found");
            }
            return post;
        }

        public IQueryable<Post> GetMany()
        {
            var posts = File.ReadAllText(filePath);
            var postList = JsonSerializer.Deserialize<List<Post>>(posts) ?? new List<Post>();
            return postList.AsQueryable();
        }
        
        

    }
}