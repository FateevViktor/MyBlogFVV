using MyBlogFVV.BLL.Models.Post;
using MyBlogFVV.BLL.Models.User;
using MyBlogFVV.WEB.Models.Comment;
using MyBlogFVV.WEB.Models.Post;
using MyBlogFVV.WEB.Models.User;


namespace MyBlogFVV.WEB.Services
{
    public class MyMappingPost
    {
        public PostRequest GetPostRequestFromPostViewModel(PostViewModel postViewModel)
        {
            PostRequest postRequest = new PostRequest();
            UserRequest userRequest = new UserRequest();

            postRequest.Id = postViewModel.Id;
            postRequest.Title = postViewModel.Title;
            postRequest.Text = postViewModel.Text;
            postRequest.PostDate = postViewModel.PostDate;

            userRequest.Id = postViewModel.Author.Id;
            userRequest.LastName = postViewModel.Author.LastName;
            userRequest.FirstName = postViewModel.Author.FirstName;
            userRequest.MiddleName = postViewModel.Author.MiddleName;
            userRequest.BirthDate = postViewModel.Author.BirthDate;
            userRequest.Email = postViewModel.Author.Email;

            postRequest.Author = userRequest;

            return postRequest;
        }
        public PostViewModel GetPostViewModelFromPostRequest(PostRequest postRequest)
        {
            PostViewModel postViewModel = new PostViewModel();

            postViewModel.Id = postRequest.Id;
            postViewModel.Title = postRequest.Title;
            postViewModel.Text = postRequest.Text;
            postViewModel.PostDate = postRequest.PostDate;

            UserViewModel userViewModel = new UserViewModel();
            userViewModel.Id = postRequest.Author.Id;
            userViewModel.FirstName = postRequest.Author.FirstName;
            userViewModel.LastName = postRequest.Author.LastName;
            userViewModel.MiddleName = postRequest.Author.MiddleName;
            userViewModel.BirthDate = postRequest.Author.BirthDate;
            userViewModel.Email = postRequest.Author.Email;

            postViewModel.Author = userViewModel;

            //Заполним комментарии к посту
            foreach (var item in postRequest.Comment)
            {
                CommentViewModel commentViewModel = new CommentViewModel();
                commentViewModel.Id = item.Id;
                commentViewModel.Text = item.Text;
                commentViewModel.CommentDate = item.CommentDate;

                //Автор
                UserViewModel userViewModel1 = new UserViewModel();
                userViewModel1.Id = item.Author.Id;
                userViewModel1.FirstName = item.Author.FirstName;
                userViewModel1.LastName = item.Author.LastName;
                userViewModel1.MiddleName = item.Author.MiddleName;
                userViewModel1.BirthDate = item.Author.BirthDate;
                userViewModel1.Email = item.Author.Email;
                //userViewModel1.Login = item.Author.Login;
                commentViewModel.Author = userViewModel1;

                postViewModel.Comment.Add(commentViewModel);
            }

            return postViewModel;
        }
        public SearchPostsViewModel GetSearchPostsViewModelFromListPostRequest(List<PostRequest> postRequest)
        {
            SearchPostsViewModel searchPostsViewModel = new SearchPostsViewModel();

            foreach (var item in postRequest)
            {
                PostViewModel postViewModel = new PostViewModel();

                postViewModel.Id = item.Id;
                postViewModel.PostDate = item.PostDate;

                UserViewModel userViewModel = new UserViewModel();
                userViewModel.Id = item.Author.Id;
                userViewModel.Login = item.Author.Login;
                userViewModel.FirstName = item.Author.FirstName;
                userViewModel.LastName = item.Author.LastName;
                userViewModel.MiddleName = item.Author.MiddleName;
                userViewModel.BirthDate = item.Author.BirthDate;
                userViewModel.Email = item.Author.Email;
                postViewModel.Author = userViewModel;

                postViewModel.Title = item.Title;
                postViewModel.Text = item.Text;

                searchPostsViewModel.PostList.Add(postViewModel);
            }

            return searchPostsViewModel;
        }
    }
}
