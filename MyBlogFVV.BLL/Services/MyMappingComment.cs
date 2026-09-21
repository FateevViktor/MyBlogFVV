
using MyBlogFVV.BLL.Models.Comment;
using MyBlogFVV.BLL.Models.Post;
using MyBlogFVV.BLL.Models.User;
using MyBlogFVV.DAL.Entities;

namespace MyBlogFVV.BLL.Services
{
    internal class MyMappingComment
    {
        public Comment GetCommentFromCommentRequest(CommentRequest commentRequest)
        {
            Comment comment = new Comment();
            comment.Text = commentRequest.Text;
            comment.Date = commentRequest.CommentDate.ToString();

            return comment;
        }
        public CommentRequest GetCommentRequestFromComment(Comment comment)
        {
            CommentRequest commentRequest = new CommentRequest();

            commentRequest.Id = comment.CommentId;
            commentRequest.Text = comment.Text;
            DateTime result;
            if (DateTime.TryParse(comment.Date, out result))
            {
                commentRequest.CommentDate = result;
            }

            return commentRequest;
        }
        public List<CommentRequest> GetListCommentRequestFromListComment(List<Comment> comment)
        {
            List<CommentRequest> listCommentRequest = new List<CommentRequest>();

            foreach (var item in comment)
            {
                CommentRequest commentRequest = new CommentRequest();
                commentRequest.Id = item.CommentId;
                commentRequest.Text = item.Text;                
                DateTime result;
                if (DateTime.TryParse(item.Date, out result))
                {
                    commentRequest.CommentDate = result;
                }

                UserRequest userRequest = new UserRequest();
                userRequest.Id = item.User.UserId;
                userRequest.Login = item.User.Login;
                userRequest.FirstName = item.User.FirstName;
                userRequest.LastName = item.User.LastName;
                userRequest.MiddleName = item.User.MiddleName;
                DateTime resultUser;
                if (DateTime.TryParse(item.User.BirthDate, out resultUser))
                {
                    userRequest.BirthDate = resultUser;
                }
                userRequest.Email = item.User.Email;
                commentRequest.Author = userRequest;

                PostRequest postRequest = new PostRequest();
                postRequest.Id = item.Post.PostId;
                postRequest.Text = item.Post.Text;
                postRequest.Title = item.Post.Title;
                DateTime resultPost;
                if (DateTime.TryParse(item.Date, out resultPost))
                {
                    postRequest.PostDate = resultUser;
                }
                commentRequest.Post = postRequest;

                listCommentRequest.Add(commentRequest);
            }

            return listCommentRequest;
        }
    }
}
