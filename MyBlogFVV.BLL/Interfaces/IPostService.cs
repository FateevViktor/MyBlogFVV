
using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Models.Post;

namespace MyBlogFVV.BLL.Interfaces
{
    public interface IPostService : IDisposable
    {
        Task<OperationDetails> Create(PostRequest postRequest); //Создание статьи
        Task<List<PostRequest>> GetAll(); //получить все статьи
        Task<List<PostRequest>> GetAll(int id); //получить все статьи определенного автора
        Task<PostRequest> GetPostById(int id); //Получить статью по Id
        Task<OperationDetails> Update(PostRequest postRequest); //Редактировать статьи
        Task<OperationDetails> Delete(int id); //Удаление статьи по его Id
    }
}
