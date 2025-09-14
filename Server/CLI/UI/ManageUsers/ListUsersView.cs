using RepositoryContract;

namespace CLI.UI.ManageUsers
{
    public class ListUsersView
    {
        private readonly IUserRepository userRepository;

        public ListUsersView(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task ShowAsync()
        {
            Console.WriteLine("\n--- List Users ---");

            var users = userRepository.GetMany().ToList();

            if (!users.Any())
            {
                Console.WriteLine("No users found.");
                return;
            }

            foreach (var user in users)
            {
                Console.WriteLine($"[{user.Id}] {user.UserName}");
            }

            await Task.CompletedTask;
        }
    }
}