
using MyBlogFVV.DAL.Entities;

namespace MyBlogFVV.DAL.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAll();
        Task<List<Post>> GetAll(int id);
        Task<Post> Get(int id);
        Task Create(Post item);
        void Update(Post item);
        void Delete(int id);
    }
}
