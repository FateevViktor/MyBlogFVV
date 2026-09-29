
using MyBlogFVV.BLL.Models.Comment;
using MyBlogFVV.BLL.Models.Post;
using MyBlogFVV.BLL.Models.User;
using MyBlogFVV.DAL.Entities;

namespace MyBlogFVV.BLL.Services
{
    internal class MyMappingComment
    {
        public static Comment GetCommentFromCommentRequest(CommentRequest commentRequest)
        {
            Comment comment = new()
            {
                Text = commentRequest.Text,
                Date = commentRequest.CommentDate.ToString()
            };

            return comment;
        }
        public static CommentRequest GetCommentRequestFromComment(Comment comment)
        {
            CommentRequest commentRequest = new()
            {
                Id = comment.CommentId,
                Text = comment.Text ?? string.Empty
            };
            if (DateTime.TryParse(comment.Date, out DateTime result))
            {
                commentRequest.CommentDate = result;
            }

            return commentRequest;
        }
        public static List<CommentRequest> GetListCommentRequestFromListComment(List<Comment> comment)
        {
            List<CommentRequest> listCommentRequest = [];

            foreach (var item in comment)
            {
                CommentRequest commentRequest = new()
                {
                    Id = item.CommentId,
                    Text = item.Text ?? string.Empty
                };
                if (DateTime.TryParse(item.Date, out DateTime result))
                {
                    commentRequest.CommentDate = result;
                }

                UserRequest userRequest = new()
                {
                    Id = item.User.UserId,
                    Login = item.User.Login,
                    FirstName = item.User.FirstName,
                    LastName = item.User.LastName ?? string.Empty,
                    MiddleName = item.User.MiddleName ?? string.Empty
                };
                if (DateTime.TryParse(item.User.BirthDate, out DateTime resultUser))
                {
                    userRequest.BirthDate = resultUser;
                }
                userRequest.Email = item.User.Email;
                commentRequest.Author = userRequest;

                PostRequest postRequest = new()
                {
                    Id = item.Post.PostId,
                    Text = item.Post.Text,
                    Title = item.Post.Title
                };
                if (DateTime.TryParse(item.Date, out DateTime resultPost))
                {
                    postRequest.PostDate = resultPost;
                }
                commentRequest.Post = postRequest;

                listCommentRequest.Add(commentRequest);
            }

            return listCommentRequest;
        }
    }
}
