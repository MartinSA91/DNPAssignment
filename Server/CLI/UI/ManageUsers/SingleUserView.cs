using RepositoryContract;

namespace CLI.UI.ManageUsers
{
    public class SingleUserView
    {
        private readonly IUserRepository userRepository;

        public SingleUserView(IUserRepository userRepository)
        {
            userRepository = userRepository;
        }

        public void Show()
        {
            Console.Write("\nEnter User ID: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out int id))
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            var user = userRepository.GetSingleAsync(id);
            if (user == null)
            {
                Console.WriteLine("User Not Found.");
                return;
            }

            Console.WriteLine($"ID: {user.Id}");
            
        }
    }
}