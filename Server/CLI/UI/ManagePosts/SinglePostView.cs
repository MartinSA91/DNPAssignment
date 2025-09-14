using RepositoryContract;

namespace CLI.UI.ManagePosts
{
    public class SinglePostView
    {
        private readonly IPostRepository postRepository;

        public SinglePostView(IPostRepository postRepository)
        {
            this.postRepository = postRepository;
        }

        public async Task ShowAsync()
        {
            Console.Write("Enter post ID: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out int postId))
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            try
            {
                var post = await postRepository.GetSingleAsync(postId);
                Console.WriteLine($"[{post.Id}] {post.Title} - {post.Body}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}