using MyBlogFVV.DAL.Entities;

namespace MyBlogFVV.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();
        Task<User?> GetUserById(int id);
        Task<User?> GetUserByEmail(string item);
        Task<User?> GetUserByLogin(string item);
        Task<User?> GetUserByLoginAndPassword(string login, string password);
        Task Create(User item);
        void Update(User item);
        void Delete(User user);
    }
}
