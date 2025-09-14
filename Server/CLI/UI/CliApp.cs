using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using RepositoryContract;

namespace CLI.UI;

public class CliApp
{
    private readonly ManagePostsView managePostsView;
    private readonly ManageUsersView manageUsersView;

    public CliApp(IUserRepository userRepository,
        ICommentRepository commentRepository,
        IPostRepository postRepository)
    {
        managePostsView = new ManagePostsView(postRepository);
        manageUsersView = new ManageUsersView(
            new CreateUserView(userRepository),
            new ListUsersView(userRepository),
            new SingleUserView(userRepository)
        );
    }

    public async Task StartAsync()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("\n--- Menu ---");
            Console.WriteLine("(1) Create Posts");
            Console.WriteLine("(2) Create Users");
            Console.WriteLine("(3) Create Comment for Post");
            Console.WriteLine("(4) List Comments for Post");
            Console.WriteLine("(0) Return");
            Console.Write("Choose: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out int selection))
            {
                Console.WriteLine("Invalid input.");
                continue;
            }

            switch (selection)
            {
                case 1:
                    managePostsView.Show();
                    break;
                case 2:
                    manageUsersView.Show();
                    break;
                case 0:
                    running = false;
                    break;
                default:
                    Console.WriteLine("Ugyldigt valg.");
                    break;
            }
        }

        await Task.CompletedTask;
    }
}