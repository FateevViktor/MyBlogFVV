using MyBlogFVV.BLL.Models.Post;
using MyBlogFVV.BLL.Models.Tag;
using MyBlogFVV.BLL.Models.User;
using MyBlogFVV.WEB.Models.Comment;
using MyBlogFVV.WEB.Models.Post;
using MyBlogFVV.WEB.Models.Tag;
using MyBlogFVV.WEB.Models.User;


namespace MyBlogFVV.WEB.Services
{
    public class MyMappingPost
    {
        public static PostRequest GetPostRequestFromPostViewModel(PostViewModel postViewModel)
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
        public static PostRequest GetPostRequestFromPostEditViewModel(PostEditViewModel postEditViewModel)
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
        public static PostRequest GetPostRequestFromPostAddViewModel(PostAddViewModel postAddViewModel)
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
        public static PostViewModel GetPostViewModelFromPostRequest(PostRequest postRequest)
        {
            PostViewModel postViewModel = new()
            {
                Id = postRequest.Id,
                Title = postRequest.Title ?? string.Empty,
                Text = postRequest.Text ?? string.Empty,
                Summary = postRequest.Summary ?? string.Empty,
                PostDate = postRequest.PostDate
            };

            UserViewModel userViewModel = new()
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
                CommentViewModel commentViewModel = new()
                {
                    Id = item.Id,
                    Text = item.Text,
                    CommentDate = item.CommentDate
                };

                //Автор
                UserViewModel userViewModel1 = new()
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
                TagViewModel tagViewModel = new()
                {
                    Id = item.Id,
                    Text = item.Text
                };
                postViewModel.Tag.Add(tagViewModel);
            }
            return postViewModel;
        }
        public static PostEditViewModel GetPostEditViewModelFromPostRequest(PostRequest postRequest, List<TagRequest> tagList)
        {
            PostEditViewModel postEditViewModel = new()
            {
                Id = postRequest.Id,
                Title = postRequest.Title ?? string.Empty,
                Text = postRequest.Text ?? string.Empty,
                Summary = postRequest.Summary ?? string.Empty,
                PostDate = postRequest.PostDate
            };

            UserViewModel userViewModel = new()
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
                CommentViewModel commentViewModel = new()
                {
                    Id = item.Id,
                    Text = item.Text,
                    CommentDate = item.CommentDate
                };

                //Автор
                UserViewModel userViewModel1 = new()
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
                TagCheckedViewModel tagCheckedViewModel = new();
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
        public static SearchPostsViewModel GetSearchPostsViewModelFromListPostRequest(List<PostRequest> postRequest)
        {
            SearchPostsViewModel searchPostsViewModel = new();

            foreach (var item in postRequest)
            {
                PostViewModel postViewModel = new()
                {
                    Id = item.Id,
                    PostDate = item.PostDate
                };

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
