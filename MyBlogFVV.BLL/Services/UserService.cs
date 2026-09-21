using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Interfaces;
using MyBlogFVV.BLL.Models.User;
using MyBlogFVV.DAL.Entities;
using MyBlogFVV.DAL.Interfaces;


namespace MyBlogFVV.BLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }
        MyMapping myMapping = new MyMapping();

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }
        //Создаем пользователя
        public async Task<OperationDetails> Create(RegisterRequest registerRequest)
        {
            if (registerRequest == null)
            {
                return new OperationDetails(false, "Передаете RegisterRequest равный null", "");
            }
            else
            {
                //Проверим, есть ли данный логин в базе
                User? userСheckLogin = await Database.Users.GetUserByLogin(registerRequest.Login);
                if (userСheckLogin != null)                
                {
                    return new OperationDetails(false, "Пользователь с таким логином уже существует", "Login");
                }
                //Проверим, есть ли данный email в базе
                User? userСheckEmail = await Database.Users.GetUserByEmail(registerRequest.Email);
                if (userСheckEmail != null)
                {
                    return new OperationDetails(false, "Пользователь с таким Email уже существует", "Email");
                }
                //Если в БД нет пользователей с таким логином и Email то заносим его в БД
                User user = myMapping.GetUserFromRegisterRequest(registerRequest);
                await Database.Users.Create(user);
                await Database.Save();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");              
            }
        }

        //Ищем пользователя по логину и паролю
        public async Task<UserRequest?> Authenticate(string login, string password)
        {
            User? user = null;
            user = await Database.Users.GetUserByLoginAndPassword(login, password);
            UserRequest? userRequest = null;
            if(user!=null)
            {
                userRequest = myMapping.GetUserRequestFromUser(user);
            }
            return userRequest;
        }
        public async Task<List<UserRequest>> GetAll()
        {
            List<UserRequest>? userRequest = new List<UserRequest>();
            List<User> users = await Database.Users.GetAll();
            if (users != null)
            {
                userRequest = myMapping.GetListUserRequestFromListUser(users);
            }
            return userRequest;
        }
        public async Task<UserRequest?> GetUserByLogin(string login)
        {
            UserRequest? userRequest = null;
            //Проверим, есть ли данный логин в базе
            User? userСheckLogin = await Database.Users.GetUserByLogin(login);
            if (userСheckLogin != null)
            {
                userRequest = myMapping.GetUserRequestFromUser(userСheckLogin);
            }
            return userRequest;
        }
        public async Task<UserRequest?> GetUserByEmail(string email)
        {
            UserRequest? userRequest = null;
            //Проверим, есть ли данный email в базе
            User? userСheck = await Database.Users.GetUserByEmail(email);
            if (userСheck != null)
            {
                userRequest = myMapping.GetUserRequestFromUser(userСheck);
            }
            return userRequest;
        }
        public async Task<UserRequest?> GetUserById(int id)
        {
            UserRequest? userRequest = null;
            //Проверим, есть ли данный Id в базе
            User? userСheck = await Database.Users.GetUserById(id);
            if (userСheck != null)
            {
                userRequest = myMapping.GetUserRequestFromUser(userСheck);
            }
            return userRequest;
        }
        public async Task<OperationDetails> Update(UserEditRequest userEditRequest)
        {
            //Проверим, есть ли данный email в базе
            User? userСheckEmail = await Database.Users.GetUserByEmail(userEditRequest.Email);
            if (userСheckEmail == null)
            {
                return new OperationDetails(false, "Пользователя с таким Email не существует", "Email");
            }
            //Теперь пробежимся по тем полям, которые изменились...
            if(userEditRequest.FirstName!= userСheckEmail.FirstName) userСheckEmail.FirstName = userEditRequest.FirstName;
            if (userEditRequest.LastName != userСheckEmail.LastName) userСheckEmail.LastName = userEditRequest.LastName;
            if (userEditRequest.MiddleName != userСheckEmail.MiddleName) userСheckEmail.MiddleName = userEditRequest.MiddleName;
            if (userEditRequest.BirthDate.ToShortDateString() != userСheckEmail.BirthDate) userСheckEmail.BirthDate = userEditRequest.BirthDate.ToShortDateString();
            if (userEditRequest.Email != userСheckEmail.Email) userСheckEmail.Email = userEditRequest.Email;
            Database.Users.Update(userСheckEmail);
            await Database.Save();
            return new OperationDetails(true, "Редактирование пользователя завершено успешно", "");

        }
        public async Task<OperationDetails> Delete(int id) //Удаление автора по его Id
        {
            //Проверим, есть ли данный Id в базе
            User? userСheckId = await Database.Users.GetUserById(id);
            if (userСheckId != null)
            {
                Database.Users.Delete(userСheckId);
                await Database.Save();
                return new OperationDetails(true, "Регистрация успешно пройдена", "MessageOperationDetails");                
            }
            else
            {
                return new OperationDetails(false, "Регистрация успешно пройдена", "");
            }
        }
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
