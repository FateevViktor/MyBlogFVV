
using System.Threading.Tasks;

namespace MyBlogFVV.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T?> Get(int id);
        Task Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}
