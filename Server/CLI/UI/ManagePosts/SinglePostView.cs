using RepositoryContract;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private readonly IPostRepository postRepository;
    
    public SinglePostView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }
    
    public void Show()
    {
        Console.Write("Enter Post ID: ");
        if (int.TryParse(Console.ReadLine(), out int postId))
        {
            var post = postRepository.GetSingleAsync(postId);
            if (post != null)
            {
                Console.WriteLine($"\n--- Post ID: {post.Id} ---");
                
            }
            else
            {
                Console.WriteLine("Post not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID format.");
        }
    }
}