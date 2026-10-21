using MyBlogFVV.BLL.Models.User;
using MyBlogFVV.API.Models.User;

namespace MyBlogFVV.API.Services
{
    public class MyMapping
    {
        public static RegisterRequest GetRegisterRequestFromRegisterModel(RegisterModel registerViewModel)
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
        public static MyPageModel GetMyPageModelFromUserRequest(UserRequest userRequest)
        {
            MyPageModel myPageViewModel = new()
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

            return myPageViewModel;
        }
        public static AuthorPageModel GetAuthorPageModelFromUserRequest(UserRequest userRequest)
        {
            AuthorPageModel authorPageViewModel = new()
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
        public static UserEditModel GetUserEditModelFromUserRequest(UserRequest userRequest)
        {
            UserEditModel userEditViewModel = new()
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


        public static SearchUsersModel GetSearchUsersModelFromListUserRequest(List<UserRequest> userRequest)
        {
            SearchUsersModel searchUsersViewModel = new();            

            foreach(var item in userRequest)
            {
                UserModel userViewModel = new()
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

        public static UserEditRequest GetUserEditRequestFromUserEditModel(UserEditModel userEditViewModel)
        {
            UserEditRequest userEditRequest = new()
            {
                FirstName = userEditViewModel.FirstName,
                LastName = userEditViewModel.LastName ?? string.Empty,
                MiddleName = userEditViewModel.MiddleName ?? string.Empty,
                Email = userEditViewModel.Email,
                BirthDate = userEditViewModel.BirthDate,
                Login = userEditViewModel.Login
            };

            return userEditRequest;
        }
    }
}
