using System.Text.Json;
using Entities;
using RepositoryContract;

namespace FileRepositories
{
    public class UserFileRepository : IUserRepository
    {
        private readonly string filePath = "users.json";

        public UserFileRepository()
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]"); 
            }
        }

        private async Task<List<User>> LoadAsync()
        {
            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        private async Task SaveAsync(List<User> users)
        {
            var json = JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(filePath, json);
        }

        public async Task<User> AddAsync(User user)
        {
            var users = await LoadAsync();
            user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
            users.Add(user);
            await SaveAsync(users);
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            var users = await LoadAsync();
            var existing = users.SingleOrDefault(u => u.Id == user.Id);
            if (existing is null)
            {
                throw new InvalidOperationException($"User with ID {user.Id} not found");
            }

            users.Remove(existing);
            users.Add(user);
            await SaveAsync(users);
        }

        public async Task DeleteAsync(int id)
        {
            var users = await LoadAsync();
            var existing = users.SingleOrDefault(u => u.Id == id);
            if (existing is null)
            {
                throw new InvalidOperationException($"User with ID {id} not found");
            }

            users.Remove(existing);
            await SaveAsync(users);
        }

        public async Task<User> GetSingleAsync(int id)
        {
            var users = await LoadAsync();
            var user = users.SingleOrDefault(u => u.Id == id);
            if (user is null)
            {
                throw new InvalidOperationException($"User with ID {id} not found");
            }

            return user;
        }

        public IQueryable<User> GetMany()
        {
            
            var users = File.ReadAllText(filePath);
            var list = JsonSerializer.Deserialize<List<User>>(users) ?? new List<User>();
            return list.AsQueryable();
        }
    }
}
