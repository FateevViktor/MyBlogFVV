
using MyBlogFVV.DAL.Entities;

namespace MyBlogFVV.DAL.Interfaces
{
    public interface ITagRepository
    {
        Task<List<Tag>> GetAll(); //Получить все теги
        Task<Tag?> GetTagById(int idTag); //Получить тег по его Id
        Task Create(Tag item);
        void Update(Tag item);
        void Delete(Tag item);
        Task<Tag?> GetTagByText(string item);
    }
}
