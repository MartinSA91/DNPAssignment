using RepositoryContract;
using Entities;

namespace CLI.UI.ManageUsers
{
    public class CreateUserView
    {
        private readonly IUserRepository userRepository;

        public CreateUserView(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task ShowAsync()
        {
            Console.Write("Enter username: ");
            var username = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Username cannot be empty.");
                return;
            }
            
            

            var user = new User { UserName = username };
            await userRepository.AddAsync(user);

            Console.WriteLine($"User '{username}' created successfully!");
        }
    }
}