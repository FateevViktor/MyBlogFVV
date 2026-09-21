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
        public CommentRequest GetCommentRequestFromCommentViewModel(CommentViewModel commentViewModel)
        {
            CommentRequest commentRequest = new CommentRequest();
            UserRequest userRequest = new UserRequest();
            PostRequest postRequest = new PostRequest();

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
        public CommentEditViewModel GetCommentEditViewModelFromCommentRequest(CommentRequest commentRequest)
        {
            CommentEditViewModel commentEditViewModel = new CommentEditViewModel();
            commentEditViewModel.Id = commentRequest.Id;
            commentEditViewModel.CommentDate = commentRequest.CommentDate;
            commentEditViewModel.Text = commentRequest.Text;

            return commentEditViewModel;
        }
        public CommentRequest GetCommentRequestFromCommentEditViewModel(CommentEditViewModel commentEditViewModel)
        {
            CommentRequest commentRequest = new CommentRequest();

            commentRequest.Id = commentEditViewModel.Id;
            commentRequest.CommentDate = commentEditViewModel.CommentDate;
            commentRequest.Text = commentEditViewModel.Text;

            return commentRequest;
        }
        public SearchCommentsViewModel GetSearchCommentsViewModelFromListCommentRequest(List<CommentRequest> commentRequest)
        {
            SearchCommentsViewModel searchCommentsViewModel = new SearchCommentsViewModel();

            foreach (var item in commentRequest)
            {
                CommentViewModel commentViewModel = new CommentViewModel();

                commentViewModel.Id = item.Id;
                commentViewModel.CommentDate = item.CommentDate;
                commentViewModel.Text = item.Text;

                //Автор комментария
                if (item.Author != null)
                {
                    UserViewModel userViewModel = new UserViewModel();
                    userViewModel.Id = item.Author.Id;
                    userViewModel.Login = item.Author.Login;
                    userViewModel.FirstName = item.Author.FirstName;
                    userViewModel.LastName = item.Author.LastName;
                    userViewModel.MiddleName = item.Author.MiddleName;
                    userViewModel.BirthDate = item.Author.BirthDate;
                    userViewModel.Email = item.Author.Email;
                    commentViewModel.Author = userViewModel;
                }

                //Пост
                if (item.Post != null)
                {
                    PostViewModel postViewModel = new PostViewModel();
                    postViewModel.Id = item.Post.Id;
                    postViewModel.Title = item.Post.Title;
                    postViewModel.Text = item.Post.Text;
                    postViewModel.PostDate = item.Post.PostDate;
                    commentViewModel.Post = postViewModel;
                }
                    searchCommentsViewModel.CommentList.Add(commentViewModel);
            }

            return searchCommentsViewModel;
        }
    }
}
