
using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Interfaces;
using MyBlogFVV.BLL.Models.Role;
using MyBlogFVV.DAL.Entities;
using MyBlogFVV.DAL.Interfaces;

namespace MyBlogFVV.BLL.Services
{
    public class RoleService(IUnitOfWork uow) : IRoleService
    {
        IUnitOfWork Database { get; set; } = uow;

        public async Task<OperationDetails> Create(RoleRequest roleRequest)
        {
            if (roleRequest == null)
            {
                return new OperationDetails(false, "Передаете RoleRequest равный null", "");
            }
            else
            {
                //Проверим, есть ли данный тег в базе
                Role? roleСheck = await Database.Roles.GetRoleByText(roleRequest.Title);
                if (roleСheck != null)
                {
                    return new OperationDetails(false, "Такая роль уже существует", "Text");
                }

                Role role = MyMappingRole.GetRoleFromRoleRequest(roleRequest);
                await Database.Roles.Create(role);
                await Database.Save();
                return new OperationDetails(true, "Роль успешно создана", "");
            }
        }

        public async Task<OperationDetails> Delete(int id)
        {
            //Проверим, есть ли данный Id в базе
            Role? roleСheckId = await Database.Roles.GetRoleById(id);
            if (roleСheckId != null)
            {
                Database.Roles.Delete(roleСheckId);
                await Database.Save();
                return new OperationDetails(true, "Удалили роль", "MessageOperationDetails");
            }
            else
            {
                return new OperationDetails(false, "Роли с таким Id не найдено", "MessageOperationDetails");
            }
        }

        public void Dispose()
        {
            Database.Dispose();
            GC.SuppressFinalize(this); // Блокируем вызов финализатора
        }

        public async Task<List<RoleRequest>> GetAll()
        {
            List<RoleRequest>? roleRequest = [];
            List<Role>? roles = await Database.Roles.GetAll();
            if (roles != null)
            {
                roleRequest = MyMappingRole.GetListRoleRequestFromListRole(roles);
            }
            return roleRequest;
        }

        public async Task<RoleRequest?> GetRoleById(int id)
        {
            RoleRequest? roleRequest = null;
            //Проверим, есть ли данный Id в базе
            Role? roleСheck = await Database.Roles.GetRoleById(id);
            if (roleСheck != null)
            {
                roleRequest = MyMappingRole.GetRoleRequestFromRole(roleСheck);
            }
            return roleRequest;
        }

        public async Task<OperationDetails> Update(RoleRequest roleRequest)
        {
            //Проверим, есть ли данная роль в базе
            Role? roleСheck = await Database.Roles.GetRoleById(roleRequest.Id);
            if (roleСheck == null)
            {
                return new OperationDetails(false, "Роли с таким Id не существует", "Id");
            }
            //Теперь пробежимся по тем полям, которые изменились...
            if (roleRequest.Title != roleСheck.Title) roleСheck.Title = roleRequest.Title;
            if (roleRequest.Description != roleСheck.Description) roleСheck.Description = roleRequest.Description;

            Database.Roles.Update(roleСheck);
            await Database.Save();
            return new OperationDetails(true, "Редактирование роли завершено успешно", "MessageOperationDetails");
        }
    }
}
