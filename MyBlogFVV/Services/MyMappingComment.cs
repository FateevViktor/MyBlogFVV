using MyBlogFVV.BLL.Models.Comment;
using MyBlogFVV.BLL.Models.Post;
using MyBlogFVV.BLL.Models.User;
using MyBlogFVV.WEB.Models.Comment;
using MyBlogFVV.WEB.Models.Post;
using MyBlogFVV.WEB.Models.User;

namespace MyBlogFVV.WEB.Services
{
    public class MyMappingComment
    {
        public static CommentRequest GetCommentRequestFromCommentViewModel(CommentViewModel commentViewModel)
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
        public static CommentEditViewModel GetCommentEditViewModelFromCommentRequest(CommentRequest commentRequest)
        {
            CommentEditViewModel commentEditViewModel = new()
            {
                Id = commentRequest.Id,
                CommentDate = commentRequest.CommentDate,
                Text = commentRequest.Text
            };

            return commentEditViewModel;
        }
        public static CommentRequest GetCommentRequestFromCommentEditViewModel(CommentEditViewModel commentEditViewModel)
        {
            CommentRequest commentRequest = new()
            {
                Id = commentEditViewModel.Id,
                CommentDate = commentEditViewModel.CommentDate,
                Text = commentEditViewModel.Text
            };

            return commentRequest;
        }
        public static SearchCommentsViewModel GetSearchCommentsViewModelFromListCommentRequest(List<CommentRequest> commentRequest)
        {
            SearchCommentsViewModel searchCommentsViewModel = new();

            foreach (var item in commentRequest)
            {
                CommentViewModel commentViewModel = new()
                {
                    Id = item.Id,
                    CommentDate = item.CommentDate,
                    Text = item.Text
                };

                //Автор комментария
                if (item.Author != null)
                {
                    UserViewModel userViewModel = new()
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
                    PostViewModel postViewModel = new()
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
