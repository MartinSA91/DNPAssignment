using RepositoryContract;

namespace CLI.UI.ManageUsers
{
    public class SingleUserView
    {
        private readonly IUserRepository userRepository;

        public SingleUserView(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task ShowAsync()
        {
            Console.Write("Enter user ID: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out int userId))
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            try
            {
                var user = await userRepository.GetSingleAsync(userId);
                Console.WriteLine($"[{user.Id}] {user.UserName}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}