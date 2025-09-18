using CLI.UI;
using FileRepositories; 
using RepositoryContract;

Console.WriteLine("Starting CLI app...");

IUserRepository userRepository = new UserFileRepository();
IPostRepository postRepository = new PostFileRepository();
ICommentRepository commentRepository = new CommentFileRepository();

var cliApp = new CliApp(userRepository, commentRepository, postRepository);
await cliApp.StartAsync();