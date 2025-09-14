using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using RepositoryContract;

namespace CLI.UI
{
    public class CliApp
    {
        private readonly ManageUsersView manageUsersView;
        private readonly ManagePostsView managePostsView;

        public CliApp(IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository)
        {
            manageUsersView = new ManageUsersView(
                new CreateUserView(userRepository),
                new ListUsersView(userRepository),
                new SingleUserView(userRepository)
            );

            managePostsView = new ManagePostsView(postRepository, commentRepository, userRepository);
        }

        public async Task StartAsync()
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("\n--- Main Menu ---");
                Console.WriteLine("1. Manage posts");
                Console.WriteLine("2. Manage users");
                Console.WriteLine("0. Exit");
                Console.Write("Select: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await managePostsView.ShowAsync();
                        break;
                    case "2":
                        await manageUsersView.ShowAsync();
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
    }
}