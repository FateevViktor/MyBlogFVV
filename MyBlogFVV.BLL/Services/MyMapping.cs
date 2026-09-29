using MyBlogFVV.BLL.Models.User;
using MyBlogFVV.DAL.Entities;

namespace MyBlogFVV.BLL.Services
{
    internal class MyMapping
    {
        public MyMapping()
        {

        }

        public static User GetUserFromRegisterRequest(RegisterRequest registerRequest)
        {
            User user = new()
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                MiddleName = registerRequest.MiddleName,
                BirthDate = registerRequest.BirthDate.ToShortDateString(),
                Email = registerRequest.Email,
                Login = registerRequest.Login,
                Password = registerRequest.Password
            };

            return user;
        }
        public static UserRequest GetUserRequestFromUser(User user)
        {
            UserRequest userRequest = new()
            {
                Id = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName ?? string.Empty,
                MiddleName = user.MiddleName ?? string.Empty
            };
            if (DateTime.TryParse(user.BirthDate, out DateTime result))
            {
                userRequest.BirthDate = result;
            }
            userRequest.Login = user.Login;
            userRequest.Email = user.Email;
            
            foreach (var role in user.UserRoles)
            {
                if (role.Role.Title!=null)
                {
                    userRequest.Roles.Add(role.Role.Title);
                }
            }
            
            return userRequest;
        }
        public static List<UserRequest> GetListUserRequestFromListUser(List<User> user)
        {            
            List<UserRequest> listUserRequest = [];

            foreach (var item in user)
            {
                UserRequest userRequest = new()
                {
                    Id = item.UserId,
                    FirstName = item.FirstName,
                    LastName = item.LastName ?? string.Empty,
                    MiddleName = item.MiddleName ?? string.Empty
                };
                if (DateTime.TryParse(item.BirthDate, out DateTime result))
                {
                    userRequest.BirthDate = result;
                }
                userRequest.Login = item.Login;
                userRequest.Email = item.Email;

                foreach (var role in item.UserRoles)
                {
                    if (role.Role.Title != null)
                    {
                        userRequest.Roles.Add(role.Role.Title);
                    }
                }
                listUserRequest.Add(userRequest);
            }

            return listUserRequest;
        }
        public static User GetUserFromUserEditRequest(UserEditRequest userEditRequest)
        {
            User user = new()
            {
                FirstName = userEditRequest.FirstName,
                LastName = userEditRequest.LastName,
                MiddleName = userEditRequest.MiddleName,
                Email = userEditRequest.Email,
                BirthDate = userEditRequest.BirthDate.ToShortDateString()
            };

            return user;
        }
    }
}
