using RepositoryContract;
using CLI.UI.ManageComments;

namespace CLI.UI.ManagePosts
{
    public class ManagePostsView
    {
        private readonly CreatePostView createPostView;
        private readonly ListPostsView listPostsView;
        private readonly SinglePostView singlePostView;
        private readonly CreateCommentView createCommentView;
        private readonly ListCommentsView listCommentsView;

        public ManagePostsView(IPostRepository postRepository, ICommentRepository commentRepository, IUserRepository userRepository)
        {
            createPostView = new CreatePostView(postRepository);
            listPostsView = new ListPostsView(postRepository);
            singlePostView = new SinglePostView(postRepository);
            createCommentView = new CreateCommentView(commentRepository, userRepository, postRepository);
            listCommentsView = new ListCommentsView(commentRepository);
        }

        public async Task ShowAsync()
        {
            while (true)
            {
                Console.WriteLine("\n--- Manage Posts ---");
                Console.WriteLine("1. Create new post");
                Console.WriteLine("2. Show all posts");
                Console.WriteLine("3. Show single post by ID");
                Console.WriteLine("4. Add comment to post");
                Console.WriteLine("5. Show comments for a post");
                Console.WriteLine("0. Back to main menu");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await createPostView.ShowAsync();
                        break;
                    case "2":
                        await listPostsView.ShowAsync();
                        break;
                    case "3":
                        await singlePostView.ShowAsync();
                        break;
                    case "4":
                        await createCommentView.ShowAsync();
                        break;
                    case "5":
                        Console.Write("Enter post ID: ");
                        if (int.TryParse(Console.ReadLine(), out int postId))
                        {
                            await listCommentsView.ShowAsync(postId);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input.");
                        }
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice, try again.");
                        break;
                }
            }
        }
    }
}
