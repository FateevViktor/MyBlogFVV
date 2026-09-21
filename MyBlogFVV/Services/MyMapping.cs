using MyBlogFVV.BLL.Models.User;
using MyBlogFVV.WEB.Models.User;

namespace MyBlogFVV.WEB.Services
{
    public class MyMapping
    {
        public RegisterRequest GetRegisterRequestFromRegisterViewModel(RegisterViewModel registerViewModel)
        {
            RegisterRequest registerRequest = new RegisterRequest();

            registerRequest.FirstName = registerViewModel.FirstName;
            registerRequest.LastName = registerViewModel.LastName;
            registerRequest.MiddleName = registerViewModel.MiddleName;
            registerRequest.Email = registerViewModel.Email;

            registerRequest.BirthDate = (DateTime)registerViewModel.BirthDate;            
            registerRequest.Password = registerViewModel.Password;
            registerRequest.Login = registerViewModel.Login;

            return registerRequest;
        }
        public MyPageViewModel GetMyPageViewModelFromUserRequest(UserRequest userRequest)
        {
            MyPageViewModel myPageViewModel = new MyPageViewModel();

            myPageViewModel.Login = userRequest.Login;
            myPageViewModel.FirstName = userRequest.FirstName;
            myPageViewModel.LastName = userRequest.LastName;
            myPageViewModel.MiddleName = userRequest.MiddleName;
            myPageViewModel.Email = userRequest.Email;
            myPageViewModel.BirthdayDate = userRequest.BirthDate;
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
        public UserEditViewModel GetUserEditViewModelFromUserRequest(UserRequest userRequest)
        {
            UserEditViewModel userEditViewModel = new UserEditViewModel();
            userEditViewModel.FirstName = userRequest.FirstName;
            userEditViewModel.LastName = userRequest.LastName;
            userEditViewModel.MiddleName = userRequest.MiddleName;
            userEditViewModel.Email = userRequest.Email;
            userEditViewModel.BirthDate = userRequest.BirthDate;

            return userEditViewModel;
        }


        public SearchUsersViewModel GetSearchUsersViewModelFromListUserRequest(List<UserRequest> userRequest)
        {
            SearchUsersViewModel searchUsersViewModel = new SearchUsersViewModel();            

            foreach(var item in userRequest)
            {
                UserViewModel userViewModel = new UserViewModel();

                userViewModel.Id = item.Id;
                userViewModel.FirstName = item.FirstName;
                userViewModel.LastName = item.LastName;
                userViewModel.MiddleName = item.MiddleName;
                userViewModel.BirthDate = item.BirthDate;
                userViewModel.Email = item.Email;

                searchUsersViewModel.UserList.Add(userViewModel);
            }         
            
            return searchUsersViewModel;
        }

        public UserEditRequest GetUserEditRequestFromUserEditViewModel(UserEditViewModel userEditViewModel)
        {
            UserEditRequest userEditRequest = new UserEditRequest();

            userEditRequest.FirstName = userEditViewModel.FirstName;
            userEditRequest.LastName = userEditViewModel.LastName;
            userEditRequest.MiddleName = userEditViewModel.MiddleName;
            userEditRequest.Email = userEditViewModel.Email;
            userEditRequest.BirthDate = (DateTime)userEditViewModel.BirthDate;

            return userEditRequest;
        }
    }
}
