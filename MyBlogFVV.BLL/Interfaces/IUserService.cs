using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Models.User;

namespace MyBlogFVV.BLL.Interfaces
{
    public interface IUserService: IDisposable
    {
        Task<OperationDetails> Create(RegisterRequest registerRequest); //Создание пользователя
        Task<UserRequest?> Authenticate(string login, string password); //Аутентификация пользователя
        Task<List<UserRequest>> GetAll(); //получить всех авторов
        Task<UserRequest?> GetUserByLogin(string login); //Получить пользователя по его логину
        Task<UserRequest?> GetUserByEmail(string email); //Получить пользователя по Email
        Task<UserRequest?> GetUserById(int id); //Получить пользователя по Id
        Task<OperationDetails> Update(UserEditRequest userEditRequest); //Редактировать пользователя
        Task<OperationDetails> Delete(int id); //Удаление автора по его Id
    }
}
