using RepositoryContract;
using Entities;

namespace CLI.UI.ManageComments
{
    public class CreateCommentView
    {
        private readonly ICommentRepository commentRepository;
        private readonly IUserRepository userRepository;
        private readonly IPostRepository postRepository;

        public CreateCommentView(ICommentRepository commentRepository, IUserRepository userRepository, IPostRepository postRepository)
        {
            this.commentRepository = commentRepository;
            this.userRepository = userRepository;
            this.postRepository = postRepository;
        }

        public async Task ShowAsync()
        {
            Console.Write("Enter user ID: ");
            if (!int.TryParse(Console.ReadLine(), out int userId) || await userRepository.GetSingleAsync(userId) == null)
            {
                Console.WriteLine("Invalid or non-existing user ID.");
                return;
            }

            Console.Write("Enter post ID: ");
            if (!int.TryParse(Console.ReadLine(), out int postId) || await postRepository.GetSingleAsync(postId) == null)
            {
                Console.WriteLine("Invalid or non-existing post ID.");
                return;
            }

            Console.Write("Enter comment: ");
            var content = Console.ReadLine();

            var comment = new Comment
            {
                UserId = userId,
                PostId = postId,
                Body = content ?? string.Empty,
                
            };

            await commentRepository.AddAsync(comment);
            Console.WriteLine("Comment added successfully!");
        }
    }
}