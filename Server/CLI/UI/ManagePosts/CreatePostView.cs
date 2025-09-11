using Entities;
using RepositoryContract;

public class CreatePostView
{
    private readonly IPostRepository postRepository;

    public CreatePostView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public void Show()
    {
        Console.WriteLine("\n--- Create New Post ---");
        Console.Write("Enter Title: ");
        var title = Console.ReadLine();

        Console.Write("Enter Content: ");
        var body = Console.ReadLine();

        var newPost = new Post
        {
            Title = title,
            Body = body,
            
        };

        postRepository.AddAsync(newPost);
        Console.WriteLine("Post created successfully.");
    }
}