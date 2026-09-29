using MyBlogFVV.BLL.Models.User;
using MyBlogFVV.WEB.Models.User;

namespace MyBlogFVV.WEB.Services
{
    public class MyMapping
    {
        public static RegisterRequest GetRegisterRequestFromRegisterViewModel(RegisterViewModel registerViewModel)
        {
            RegisterRequest registerRequest = new()
            {
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName ?? string.Empty,
                MiddleName = registerViewModel.MiddleName ?? string.Empty,
                Email = registerViewModel.Email,
                BirthDate = /*(DateTime)*/registerViewModel.BirthDate,
                Password = registerViewModel.Password,
                Login = registerViewModel.Login
            };

            return registerRequest;
        }
        public static MyPageViewModel GetMyPageViewModelFromUserRequest(UserRequest userRequest)
        {
            MyPageViewModel myPageViewModel = new()
            {
                Login = userRequest.Login,
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                MiddleName = userRequest.MiddleName,
                Email = userRequest.Email,
                BirthdayDate = userRequest.BirthDate
            };
            foreach (string role in userRequest.Roles)
            {
                if (role != null)
                {
                    myPageViewModel.Roles.Add(role);
                }
            }

            /*
            public List<PostViewModel> Posts { get; set; } = new List<PostViewModel>();
            public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
            public List<TagViewModel> Tags { get; set; } = new List<TagViewModel>();
            */

            return myPageViewModel;
        }
        public static AuthorPageViewModel GetAuthorPageViewModelFromUserRequest(UserRequest userRequest)
        {
            AuthorPageViewModel authorPageViewModel = new()
            {
                Id = userRequest.Id,
                Login = userRequest.Login,
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                MiddleName = userRequest.MiddleName,
                Email = userRequest.Email,
                BirthdayDate = userRequest.BirthDate
            };
            foreach (string role in userRequest.Roles)
            {
                if (role != null)
                {
                    authorPageViewModel.Roles.Add(role);
                }
            }

            return authorPageViewModel;
        }
        public static UserEditViewModel GetUserEditViewModelFromUserRequest(UserRequest userRequest)
        {
            UserEditViewModel userEditViewModel = new()
            {
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                MiddleName = userRequest.MiddleName,
                Email = userRequest.Email,
                BirthDate = userRequest.BirthDate,
                Login = userRequest.Login
            };

            foreach (var item in userRequest.Roles)
            {
                userEditViewModel.Roles.Add(item);
            }

            return userEditViewModel;
        }


        public static SearchUsersViewModel GetSearchUsersViewModelFromListUserRequest(List<UserRequest> userRequest)
        {
            SearchUsersViewModel searchUsersViewModel = new();            

            foreach(var item in userRequest)
            {
                UserViewModel userViewModel = new()
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    MiddleName = item.MiddleName,
                    BirthDate = item.BirthDate,
                    Email = item.Email
                };

                searchUsersViewModel.UserList.Add(userViewModel);
            }         
            
            return searchUsersViewModel;
        }

        public static UserEditRequest GetUserEditRequestFromUserEditViewModel(UserEditViewModel userEditViewModel)
        {
            UserEditRequest userEditRequest = new()
            {
                FirstName = userEditViewModel.FirstName,
                LastName = userEditViewModel.LastName ?? string.Empty,
                MiddleName = userEditViewModel.MiddleName ?? string.Empty,
                Email = userEditViewModel.Email,
                BirthDate = /*(DateTime)*/userEditViewModel.BirthDate,
                Login = userEditViewModel.Login
            };

            return userEditRequest;
        }
    }
}
