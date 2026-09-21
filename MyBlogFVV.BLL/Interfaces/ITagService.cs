
using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Models.Tag;

namespace MyBlogFVV.BLL.Interfaces
{
    public interface ITagService : IDisposable
    {
        Task<OperationDetails> Create(TagRequest tagRequest); //Создание тега
        Task<List<TagRequest>> GetAll(); //получить все теги
        Task<TagRequest?> GetTagById(int id); //Получить тег по Id
        Task<OperationDetails> Update(TagRequest tagRequest); //Редактировать тег
        Task<OperationDetails> Delete(int id); //Удаление тега по его Id
    }
}
