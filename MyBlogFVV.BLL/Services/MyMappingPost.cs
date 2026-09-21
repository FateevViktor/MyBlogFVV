
using MyBlogFVV.BLL.Models.Comment;
using MyBlogFVV.BLL.Models.Post;
using MyBlogFVV.BLL.Models.User;
using MyBlogFVV.DAL.Entities;

namespace MyBlogFVV.BLL.Services
{
    internal class MyMappingPost
    {
        public Post GetPostFromPostRequest(PostRequest postRequest)
        {
            Post post = new Post();
            post.Title = postRequest.Title;
            post.Text = postRequest.Text;
            post.Date = postRequest.PostDate.ToString();

            return post;
        }
        public PostRequest GetPostRequestFromPost(Post post)
        {
            PostRequest postRequest = new PostRequest();
            postRequest.Id = post.PostId;
            postRequest.Title = post.Title;
            postRequest.Text = post.Text;          

            DateTime result;
            if (DateTime.TryParse(post.Date, out result))
            {
                postRequest.PostDate = result;
            }

            UserRequest userRequest = new UserRequest();
            userRequest.Id = post.User.UserId;
            userRequest.Login = post.User.Login;
            userRequest.FirstName = post.User.FirstName;
            userRequest.LastName = post.User.LastName;
            userRequest.MiddleName = post.User.MiddleName;
            DateTime resultUser;
            if (DateTime.TryParse(post.User.BirthDate, out resultUser))
            {
                userRequest.BirthDate = resultUser;
            }
            userRequest.Email = post.User.Email;
            postRequest.Author = userRequest;

            //Заполним комментарии к посту
            foreach (var item in post.Comments)
            {
                CommentRequest commentRequest = new CommentRequest();
                commentRequest.Id = item.CommentId;
                commentRequest.Text = item.Text;
                DateTime result1;
                if (DateTime.TryParse(item.Date, out result1))
                {
                    commentRequest.CommentDate = result1;
                }

                //Автор
                UserRequest userRequest1 = new UserRequest();
                userRequest1.Id = item.User.UserId;
                userRequest1.FirstName = item.User.FirstName;
                userRequest1.LastName = item.User.LastName;
                userRequest1.MiddleName = item.User.MiddleName;
                DateTime result2;
                if (DateTime.TryParse(item.Date, out result2))
                {
                    userRequest1.BirthDate = result2;
                }
                userRequest1.Email = item.User.Email;
                userRequest1.Login = item.User.Login;
                commentRequest.Author = userRequest1;

                postRequest.Comment.Add(commentRequest);
            }

            return postRequest;
        }

        public List<PostRequest> GetListPostRequestFromListPost(List<Post> post)
        {
            List<PostRequest> listPostRequest = new List<PostRequest>();

            foreach (var item in post)
            {
                PostRequest postRequest = new PostRequest();
                postRequest.Id = item.PostId;
                postRequest.Title = item.Title;
                postRequest.Text = item.Text;
                DateTime result;
                if (DateTime.TryParse(item.Date, out result))
                {
                    postRequest.PostDate = result;
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
                postRequest.Author = userRequest;

                listPostRequest.Add(postRequest);
            }

            return listPostRequest;
        }
    }
}
