
using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Models.Role;

namespace MyBlogFVV.BLL.Interfaces
{
    public interface IRoleService : IDisposable
    {
        Task<OperationDetails> Create(RoleRequest roleRequest); //Создание роли
        Task<List<RoleRequest>> GetAll(); //получить все роли
        Task<RoleRequest?> GetRoleById(int id); //Получить роль по Id
        Task<OperationDetails> Update(RoleRequest roleRequest); //Редактировать роль
        Task<OperationDetails> Delete(int id); //Удаление роли по её Id
    }
}
