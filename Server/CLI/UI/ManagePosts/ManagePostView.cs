using System;
using RepositoryContract;

namespace CLI.UI.ManagePosts
{
    public class ManagePostsView
    {
        private readonly CreatePostView createPostView;
        private readonly ListPostsView listPostsView;
        private readonly SinglePostView singlePostView;

        public ManagePostsView(IPostRepository postRepo)
        {
            createPostView = new CreatePostView(postRepo);
            listPostsView = new ListPostsView(postRepo);
            singlePostView = new SinglePostView(postRepo);
        }

        public void Show()
        {
            while (true)
            {
                Console.WriteLine("\n--- Manage Posts ---");
                Console.WriteLine("1. Create new post");
                Console.WriteLine("2. Show all posts");
                Console.WriteLine("3. show single post by ID");
                Console.WriteLine("0. Back to main menu");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        createPostView.Show();
                        break;
                    case "2":
                        listPostsView.Show();
                        break;
                    case "3":
                        singlePostView.Show();
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