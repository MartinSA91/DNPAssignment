using RepositoryContract;

namespace CLI.UI.ManageUsers
{
    public class ListUsersView
    {
        private readonly IUserRepository userRepository;

        public ListUsersView(IUserRepository userRepository)
        {
            userRepository = userRepository;
        }

        public void Show()
        {
            Console.WriteLine("\n--- List of Users ---");

            var users = userRepository.GetMany();

            if (!users.Any())
            {
                Console.WriteLine("No Users Found.");
                return;
            }

            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.Id}, Navn: {user.UserName}");
            }
        }
    }
}