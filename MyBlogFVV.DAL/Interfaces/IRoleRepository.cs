
using MyBlogFVV.DAL.Entities;

namespace MyBlogFVV.DAL.Interfaces
{
    public interface IRoleRepository
    {
        Task<List<Role>> GetAll(); //Получить все роли
        Task<Role?> GetRoleById(int idRole); //Получить роль по его Id
        Task Create(Role item);
        void Update(Role item);
        void Delete(Role item);
        Task<Role?> GetRoleByText(string item); //Получить роль по её названию
    }
}
