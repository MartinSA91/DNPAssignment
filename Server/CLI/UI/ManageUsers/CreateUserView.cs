using RepositoryContract;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepository;
    
    public CreateUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }
    
    public void Show()
    {
        Console.WriteLine("\n--- Create New User ---");
        Console.Write("Enter Username: ");
        string username = Console.ReadLine();
        Console.Write("Enter Password: ");
        string password = Console.ReadLine();
        
        var newUser = new Entities.User
        {
            UserName = username,
            Password = password
        };
        
        userRepository.AddAsync(newUser).Wait();
        Console.WriteLine("User created successfully.");
    }
    
    
    
}