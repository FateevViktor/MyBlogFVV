
using MyBlogFVV.BLL.Models.Comment;
using MyBlogFVV.BLL.Models.Post;
using MyBlogFVV.BLL.Models.Tag;
using MyBlogFVV.BLL.Models.User;
using MyBlogFVV.DAL.Entities;

namespace MyBlogFVV.BLL.Services
{
    internal class MyMappingPost
    {
        public static Post GetPostFromPostRequest(PostRequest postRequest)
        {
            Post post = new()
            {
                Title = postRequest.Title,
                Text = postRequest.Text,
                Summary = postRequest.Summary,
                Date = postRequest.PostDate.ToString()
            };

            return post;
        }
        public static PostRequest GetPostRequestFromPost(Post post)
        {
            PostRequest postRequest = new()
            {
                Id = post.PostId,
                Title = post.Title,
                Text = post.Text,
                Summary = post.Summary
            };

            if (DateTime.TryParse(post.Date, out DateTime result))
            {
                postRequest.PostDate = result;
            }

            UserRequest userRequest = new();
            if (post.User is not null)
            {
                userRequest.Id = post.User.UserId;
                userRequest.Login = post.User.Login;
                userRequest.FirstName = post.User.FirstName;
                userRequest.LastName = post.User.LastName ?? string.Empty;
                userRequest.MiddleName = post.User.MiddleName ?? string.Empty;
                if (DateTime.TryParse(post.User.BirthDate, out DateTime resultUser))
                {
                    userRequest.BirthDate = resultUser;
                }
                userRequest.Email = post.User.Email;
                postRequest.Author = userRequest;
            }

            //Заполним комментарии к посту
            foreach (var item in post.Comments)
            {
                CommentRequest commentRequest = new()
                {
                    Id = item.CommentId,
                    Text = item.Text ?? string.Empty
                };
                if (DateTime.TryParse(item.Date, out DateTime result1))
                {
                    commentRequest.CommentDate = result1;
                }

                //Автор
                UserRequest userRequest1 = new()
                {
                    Id = item.User.UserId,
                    FirstName = item.User.FirstName,
                    LastName = item.User.LastName ?? string.Empty,
                    MiddleName = item.User.MiddleName ?? string.Empty
                };
                if (DateTime.TryParse(item.Date, out DateTime result2))
                {
                    userRequest1.BirthDate = result2;
                }
                userRequest1.Email = item.User.Email;
                userRequest1.Login = item.User.Login;
                commentRequest.Author = userRequest1;

                postRequest.Comment.Add(commentRequest);
            }

            //Заполним теги
            foreach (var item in post.PostTags)
            {
                TagRequest tagRequest = new()
                {
                    Id = item.TagId,
                    Text = item.Tag.Text ?? string.Empty
                };
                postRequest.Tag.Add(tagRequest);
            }

            return postRequest;
        }

        public static List<PostRequest> GetListPostRequestFromListPost(List<Post> post)
        {
            List<PostRequest> listPostRequest = [];

            foreach (var item in post)
            {
                PostRequest postRequest = new()
                {
                    Id = item.PostId,
                    Title = item.Title,
                    Text = item.Text,
                    Summary = item.Summary
                };
                if (DateTime.TryParse(item.Date, out DateTime result))
                {
                    postRequest.PostDate = result;
                }


                UserRequest userRequest = new();
                if (item.User != null)
                {
                    userRequest.Id = item.User.UserId;
                    userRequest.Login = item.User.Login;
                    userRequest.FirstName = item.User.FirstName;
                    userRequest.LastName = item.User.LastName ?? string.Empty;
                    userRequest.MiddleName = item.User.MiddleName ?? string.Empty;
                    if (DateTime.TryParse(item.User.BirthDate, out DateTime resultUser))
                    {
                        userRequest.BirthDate = resultUser;
                    }
                    userRequest.Email = item.User.Email;
                    postRequest.Author = userRequest;
                }

                listPostRequest.Add(postRequest);
            }

            return listPostRequest;
        }
    }
}
