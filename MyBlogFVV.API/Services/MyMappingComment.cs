using MyBlogFVV.BLL.Models.Comment;
using MyBlogFVV.BLL.Models.Post;
using MyBlogFVV.BLL.Models.User;
using MyBlogFVV.API.Models.Comment;
using MyBlogFVV.API.Models.Post;
using MyBlogFVV.API.Models.User;

namespace MyBlogFVV.API.Services
{
    public class MyMappingComment
    {
        public static CommentRequest GetCommentRequestFromCommentModel(CommentModel commentViewModel)
        {
            CommentRequest commentRequest = new();
            UserRequest userRequest = new();
            PostRequest postRequest = new();

            commentRequest.Id = commentViewModel.Id;
            commentRequest.CommentDate = commentViewModel.CommentDate;
            commentRequest.Text = commentViewModel.Text;

            postRequest.Id = commentViewModel.Post.Id;
            postRequest.Title = commentViewModel.Post.Title;
            postRequest.Text = commentViewModel.Post.Text;
            postRequest.PostDate = commentViewModel.Post.PostDate;

            userRequest.Id = commentViewModel.Author.Id;
            userRequest.LastName = commentViewModel.Author.LastName;
            userRequest.FirstName = commentViewModel.Author.FirstName;
            userRequest.MiddleName = commentViewModel.Author.MiddleName;
            userRequest.BirthDate = commentViewModel.Author.BirthDate;
            userRequest.Email = commentViewModel.Author.Email;

            commentRequest.Author = userRequest;
            commentRequest.Post = postRequest;

            return commentRequest;
        }
        public static CommentEditModel GetCommentEditModelFromCommentRequest(CommentRequest commentRequest)
        {
            CommentEditModel commentEditViewModel = new()
            {
                Id = commentRequest.Id,
                CommentDate = commentRequest.CommentDate,
                Text = commentRequest.Text
            };

            return commentEditViewModel;
        }
        public static CommentRequest GetCommentRequestFromCommentEditModel(CommentEditModel commentEditViewModel)
        {
            CommentRequest commentRequest = new()
            {
                Id = commentEditViewModel.Id,
                CommentDate = commentEditViewModel.CommentDate,
                Text = commentEditViewModel.Text
            };

            return commentRequest;
        }
        public static SearchCommentsModel GetSearchCommentsModelFromListCommentRequest(List<CommentRequest> commentRequest)
        {
            SearchCommentsModel searchCommentsViewModel = new();

            foreach (var item in commentRequest)
            {
                CommentModel commentViewModel = new()
                {
                    Id = item.Id,
                    CommentDate = item.CommentDate,
                    Text = item.Text
                };

                //Автор комментария
                if (item.Author != null)
                {
                    UserModel userViewModel = new()
                    {
                        Id = item.Author.Id,
                        Login = item.Author.Login,
                        FirstName = item.Author.FirstName,
                        LastName = item.Author.LastName,
                        MiddleName = item.Author.MiddleName,
                        BirthDate = item.Author.BirthDate,
                        Email = item.Author.Email
                    };
                    commentViewModel.Author = userViewModel;
                }

                //Пост
                if (item.Post != null)
                {
                    PostModel postViewModel = new()
                    {
                        Id = item.Post.Id,
                        Title = item.Post.Title ?? string.Empty,
                        Text = item.Post.Text ?? string.Empty,
                        PostDate = item.Post.PostDate
                    };
                    commentViewModel.Post = postViewModel;
                }
                    searchCommentsViewModel.CommentList.Add(commentViewModel);
            }

            return searchCommentsViewModel;
        }
    }
}
