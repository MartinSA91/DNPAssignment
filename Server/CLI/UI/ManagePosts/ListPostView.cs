using RepositoryContract;

namespace CLI.UI.ManagePosts;

public class ListPostsView
{
    private readonly IPostRepository postRepository;

    public ListPostsView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }
    
    public void Show()
    {
        var posts = postRepository.GetMany();
        Console.WriteLine("\n--- All Posts ---");
        foreach (var post in posts)
        {
            Console.WriteLine($"ID: {post.Id}, Title: {post.Title}");
        }
    }
}