
using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Models.Comment;

namespace MyBlogFVV.BLL.Interfaces
{
    public interface ICommentService : IDisposable
    {
        Task<OperationDetails> Create(CommentRequest commentRequest); //Создание комментария
        Task<List<CommentRequest>> GetAll(); //получить все комментарии
        Task<CommentRequest?> GetCommentById(int id); //Получить комментарий по Id
        Task<OperationDetails> Update(CommentRequest commentRequest); //Редактировать комментарий
        Task<OperationDetails> Delete(int id); //Удаление комментария по его Id
    }
}
