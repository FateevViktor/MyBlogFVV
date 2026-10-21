using MyBlogFVV.BLL.Models.Post;
using MyBlogFVV.BLL.Models.Tag;
using MyBlogFVV.BLL.Models.User;
using MyBlogFVV.API.Models.Comment;
using MyBlogFVV.API.Models.Post;
using MyBlogFVV.API.Models.Tag;
using MyBlogFVV.API.Models.User;


namespace MyBlogFVV.API.Services
{
    public class MyMappingPost
    {
        public static PostRequest GetPostRequestFromPostModel(PostModel postViewModel)
        {
            PostRequest postRequest = new();
            UserRequest userRequest = new();

            postRequest.Id = postViewModel.Id;
            postRequest.Title = postViewModel.Title;
            postRequest.Text = postViewModel.Text;
            postRequest.Summary = postViewModel.Summary;
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
        public static PostRequest GetPostRequestFromPostEditModel(PostEditModel postEditViewModel)
        {
            PostRequest postRequest = new();
            UserRequest userRequest = new();

            postRequest.Id = postEditViewModel.Id;
            postRequest.Title = postEditViewModel.Title;
            postRequest.Text = postEditViewModel.Text;
            postRequest.Summary = postEditViewModel.Summary;
            postRequest.PostDate = postEditViewModel.PostDate;

            userRequest.Id = postEditViewModel.Author.Id;
            userRequest.LastName = postEditViewModel.Author.LastName;
            userRequest.FirstName = postEditViewModel.Author.FirstName;
            userRequest.MiddleName = postEditViewModel.Author.MiddleName;
            userRequest.BirthDate = postEditViewModel.Author.BirthDate;
            userRequest.Email = postEditViewModel.Author.Email;

            postRequest.Author = userRequest;

            //Подгрузим теги
            foreach (var item in postEditViewModel.Tag)
            {
                if (item.Checked == true)
                {
                    TagRequest tagRequest = new()
                    {
                        Id = item.Id,
                        Text = item.Text
                    };
                    postRequest.Tag.Add(tagRequest);
                }
            }

            return postRequest;
        }
        public static PostRequest GetPostRequestFromPostAddModel(PostAddModel postAddViewModel)
        {
            PostRequest postRequest = new();
            UserRequest userRequest = new();            

            postRequest.Id = postAddViewModel.Id;
            postRequest.Title = postAddViewModel.Title;
            postRequest.Text = postAddViewModel.Text;
            postRequest.Summary = postAddViewModel.Summary;
            postRequest.PostDate = postAddViewModel.PostDate;

            userRequest.Id = postAddViewModel.Author.Id;
            userRequest.LastName = postAddViewModel.Author.LastName;
            userRequest.FirstName = postAddViewModel.Author.FirstName;
            userRequest.MiddleName = postAddViewModel.Author.MiddleName;
            userRequest.BirthDate = postAddViewModel.Author.BirthDate;
            userRequest.Email = postAddViewModel.Author.Email;

            postRequest.Author = userRequest;

            foreach (var item in postAddViewModel.Tag)
            {
                if(item.Checked == true)
                {
                    TagRequest tagRequest = new()
                    {
                        Id = item.Id,
                        Text = item.Text
                    };
                    postRequest.Tag.Add(tagRequest);
                }
            }

            return postRequest;
        }
        public static PostModel GetPostModelFromPostRequest(PostRequest postRequest)
        {
            PostModel postViewModel = new()
            {
                Id = postRequest.Id,
                Title = postRequest.Title ?? string.Empty,
                Text = postRequest.Text ?? string.Empty,
                Summary = postRequest.Summary ?? string.Empty,
                PostDate = postRequest.PostDate
            };

            UserModel userViewModel = new()
            {
                Id = postRequest.Author.Id,
                FirstName = postRequest.Author.FirstName,
                LastName = postRequest.Author.LastName,
                MiddleName = postRequest.Author.MiddleName,
                BirthDate = postRequest.Author.BirthDate,
                Email = postRequest.Author.Email
            };

            postViewModel.Author = userViewModel;

            //Заполним комментарии к посту
            foreach (var item in postRequest.Comment)
            {
                CommentModel commentViewModel = new()
                {
                    Id = item.Id,
                    Text = item.Text,
                    CommentDate = item.CommentDate
                };

                //Автор
                UserModel userViewModel1 = new()
                {
                    Id = item.Author.Id,
                    FirstName = item.Author.FirstName,
                    LastName = item.Author.LastName,
                    MiddleName = item.Author.MiddleName,
                    BirthDate = item.Author.BirthDate,
                    Email = item.Author.Email
                };
                //userViewModel1.Login = item.Author.Login;
                commentViewModel.Author = userViewModel1;

                postViewModel.Comment.Add(commentViewModel);
            }

            //Заполним теги
            foreach (var item in postRequest.Tag)
            {
                TagModel tagViewModel = new()
                {
                    Id = item.Id,
                    Text = item.Text
                };
                postViewModel.Tag.Add(tagViewModel);
            }
            return postViewModel;
        }
        public static PostEditModel GetPostEditModelFromPostRequest(PostRequest postRequest, List<TagRequest> tagList)
        {
            PostEditModel postEditViewModel = new()
            {
                Id = postRequest.Id,
                Title = postRequest.Title ?? string.Empty,
                Text = postRequest.Text ?? string.Empty,
                Summary = postRequest.Summary ?? string.Empty,
                PostDate = postRequest.PostDate
            };

            UserModel userViewModel = new()
            {
                Id = postRequest.Author.Id,
                FirstName = postRequest.Author.FirstName,
                LastName = postRequest.Author.LastName,
                MiddleName = postRequest.Author.MiddleName,
                BirthDate = postRequest.Author.BirthDate,
                Email = postRequest.Author.Email
            };

            postEditViewModel.Author = userViewModel;

            //Заполним комментарии к посту
            foreach (var item in postRequest.Comment)
            {
                CommentModel commentViewModel = new()
                {
                    Id = item.Id,
                    Text = item.Text,
                    CommentDate = item.CommentDate
                };

                //Автор
                UserModel userViewModel1 = new()
                {
                    Id = item.Author.Id,
                    FirstName = item.Author.FirstName,
                    LastName = item.Author.LastName,
                    MiddleName = item.Author.MiddleName,
                    BirthDate = item.Author.BirthDate,
                    Email = item.Author.Email
                };
                //userViewModel1.Login = item.Author.Login;
                commentViewModel.Author = userViewModel1;

                postEditViewModel.Comment.Add(commentViewModel);
            }

            bool check;
            //Заполним теги
            foreach (var itemAll in tagList)
            {
                TagCheckedModel tagCheckedViewModel = new();
                check = false;
                foreach (var itemPost in postRequest.Tag)
                {                    
                    if (itemPost.Text == itemAll.Text)
                    {
                        check = true;
                        break;
                    }
                }
                tagCheckedViewModel.Id = itemAll.Id;
                tagCheckedViewModel.Text = itemAll.Text;
                if (check == true)
                {                 
                    tagCheckedViewModel.Checked = true;
                }
                else
                {
                    tagCheckedViewModel.Checked = false;
                }
                postEditViewModel.Tag.Add(tagCheckedViewModel);
            }

            return postEditViewModel;
        }
        public static SearchPostsModel GetSearchPostsModelFromListPostRequest(List<PostRequest> postRequest)
        {
            SearchPostsModel searchPostsViewModel = new();

            foreach (var item in postRequest)
            {
                PostModel postViewModel = new()
                {
                    Id = item.Id,
                    PostDate = item.PostDate
                };

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
                postViewModel.Author = userViewModel;

                postViewModel.Title = item.Title ?? string.Empty;
                postViewModel.Text = item.Text ?? string.Empty;
                postViewModel.Summary = item.Summary ?? string.Empty;

                searchPostsViewModel.PostList.Add(postViewModel);
            }

            return searchPostsViewModel;
        }
    }
}
