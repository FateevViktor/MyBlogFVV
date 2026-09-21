
using MyBlogFVV.DAL.Entities;

namespace MyBlogFVV.DAL.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAll(); //Получить все комменты
        Task<List<Comment>> GetAllUser(int idUser); //Получить все комменты Id автора
        Task<List<Comment>> GetAllPost(int idPost); //Получить все комменты Id поста
        Task<Comment> GetCommentById(int idComment); //Получить коммент по его Id
        Task Create(Comment item);
        void Update(Comment item);
        void Delete(Comment item);
    }
}
