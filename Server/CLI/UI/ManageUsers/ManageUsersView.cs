namespace CLI.UI.ManageUsers;

public class ManageUsersView
{
    private readonly CreateUserView createUserView;
    private readonly ListUsersView listUsersView;
    private readonly SingleUserView singleUserView;
    
    
    
    public ManageUsersView(CreateUserView createUserView, ListUsersView listUsersView, SingleUserView singleUserView)
    {
        this.createUserView = createUserView;
        this.listUsersView = listUsersView;
        this.singleUserView = singleUserView;
    }
    
    
    public Task Show()
    {
        while (true)
        {
            Console.WriteLine("\n--- Manage Users ---");
            Console.WriteLine("1. Create new user");
            Console.WriteLine("0. Back to main menu");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    createUserView.Show();
                    break;
                case "0":
                    return Task.CompletedTask;
                default:
                    Console.WriteLine("Invalid choice, try again.");
                    break;
            }
        }
    }
}