using MyBlogFVV.BLL.Models.User;
using MyBlogFVV.DAL.Entities;

namespace MyBlogFVV.BLL.Services
{
    internal class MyMapping
    {
        public MyMapping()
        {

        }

        public User GetUserFromRegisterRequest(RegisterRequest registerRequest)
        {
            User user = new User();

            user.FirstName = registerRequest.FirstName;
            user.LastName = registerRequest.LastName;
            user.MiddleName = registerRequest.MiddleName;
            user.BirthDate = registerRequest.BirthDate.ToShortDateString();
            user.Email = registerRequest.Email;
            user.Login = registerRequest.Login;
            user.Password = registerRequest.Password;

            return user;
        }
        public UserRequest GetUserRequestFromUser(User user)
        {
            UserRequest userRequest = new UserRequest();

            userRequest.Id = user.UserId;
            userRequest.FirstName = user.FirstName;
            userRequest.LastName = user.LastName;
            userRequest.MiddleName = user.MiddleName;
            DateTime result;
            if (DateTime.TryParse(user.BirthDate, out result))
            {
                userRequest.BirthDate = result;
            }
            userRequest.Login = user.Login;
            userRequest.Email = user.Email;
            
            foreach (var role in user.UserRoles)
            {
                if (role != null)
                {
                    userRequest.Roles.Add(role.Role.Text);
                }
            }
            
            return userRequest;
        }
        public List<UserRequest> GetListUserRequestFromListUser(List<User> user)
        {            
            List<UserRequest> listUserRequest = new List<UserRequest>();

            foreach (var item in user)
            {
                UserRequest userRequest = new UserRequest();
                userRequest.Id = item.UserId;
                userRequest.FirstName = item.FirstName;
                userRequest.LastName = item.LastName;
                userRequest.MiddleName = item.MiddleName;
                DateTime result;
                if (DateTime.TryParse(item.BirthDate, out result))
                {
                    userRequest.BirthDate = result;
                }
                userRequest.Login = item.Login;
                userRequest.Email = item.Email;

                foreach (var role in item.UserRoles)
                {
                    if (role != null)
                    {
                        userRequest.Roles.Add(role.Role.Text);
                    }
                }
                listUserRequest.Add(userRequest);
            }

            return listUserRequest;
        }
        public User GetUserFromUserEditRequest(UserEditRequest userEditRequest)
        {
            User user = new User();

            user.FirstName = userEditRequest.FirstName;
            user.LastName = userEditRequest.LastName;
            user.MiddleName = userEditRequest.MiddleName;
            user.Email = userEditRequest.Email;
            user.BirthDate = userEditRequest.BirthDate.ToShortDateString();

            return user;
        }
    }
}
