using System;
using CLI.UI.ManageUsers;

namespace CLI.UI.ManageUsers
{
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

        public async Task ShowAsync()
        {
            while (true)
            {
                Console.WriteLine("\n--- Manage Users ---");
                Console.WriteLine("1. Create new user");
                Console.WriteLine("2. List users");
                Console.WriteLine("3. Show single user by ID");
                Console.WriteLine("0. Back to main menu");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await createUserView.ShowAsync();
                        break;
                    case "2":
                        await listUsersView.ShowAsync();
                        break;
                    case "3":
                        await singleUserView.ShowAsync();
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