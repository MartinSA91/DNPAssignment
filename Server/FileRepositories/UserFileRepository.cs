using Entities;
using RepositoryContract;
using System.Text.Json;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "users.json";
    private List<User> users = new();

    public UserFileRepository()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }
    }

    private async Task SaveAsync()
    {
        string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task<User> AddAsync(User user)
    {
        user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
        users.Add(user);
        await SaveAsync();
        return user;
    }

    public async Task<User> GetSingleAsync(int id)
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user == null) throw new InvalidOperationException("User not found");
        return await Task.FromResult(user);
    }

    public IQueryable<User> GetMany()
    {
        return users.AsQueryable();
    }

    public async Task UpdateAsync(User user)
    {
        var existing = users.FirstOrDefault(u => u.Id == user.Id);
        if (existing == null) throw new InvalidOperationException("User not found");

        existing.UserName = user.UserName;
        existing.Password = user.Password;
        await SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user == null) throw new InvalidOperationException("User not found");

        users.Remove(user);
        await SaveAsync();
    }
}