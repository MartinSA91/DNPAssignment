using RepositoryContract;
using Entities;

namespace CLI.UI.ManagePosts
{
    public class CreatePostView
    {
        private readonly IPostRepository postRepository;

        public CreatePostView(IPostRepository postRepository)
        {
            this.postRepository = postRepository;
        }

        public async Task ShowAsync()
        {
            Console.Write("Enter post title: ");
            var title = Console.ReadLine();

            Console.Write("Enter post content: ");
            var body = Console.ReadLine();

            var post = new Post
            {
                Title = title ?? string.Empty,
                Body = body ?? string.Empty,
                
            };

            await postRepository.AddAsync(post);
            Console.WriteLine("Post created successfully!");
        }
    }
}