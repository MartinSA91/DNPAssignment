using RepositoryContract;

namespace CLI.UI.ManageComments
{
    public class ListCommentsView
    {
        private readonly ICommentRepository commentRepository;

        public ListCommentsView(ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
        }

        public async Task ShowAsync(int postId)
        {
            Console.WriteLine($"\n--- Comments for post {postId} ---");

            var comments = commentRepository.GetMany().Where(c => c.PostId == postId).ToList();

            if (!comments.Any())
            {
                Console.WriteLine("No comments yet.");
                return;
            }

            foreach (var comment in comments)
            {
                Console.WriteLine($"[{comment.Id}] User {comment.UserId}: {comment.Body}");
            }

            await Task.CompletedTask; 
        }
    }
}