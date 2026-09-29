
using MyBlogFVV.BLL.Models.Role;
using MyBlogFVV.DAL.Entities;

namespace MyBlogFVV.BLL.Services
{
    internal class MyMappingRole
    {
        public static Role GetRoleFromRoleRequest(RoleRequest roleRequest)
        {
            Role role = new()
            {
                RoleId = roleRequest.Id,
                Title = roleRequest.Title,
                Description = roleRequest.Description
            };
            return role;
        }
        public static RoleRequest GetRoleRequestFromRole(Role role)
        {
            RoleRequest roleRequest = new()
            {
                Id = role.RoleId,
                Title = role.Title ?? string.Empty,
                Description = role.Description ?? string.Empty
            };
            return roleRequest;
        }
        public static List<RoleRequest> GetListRoleRequestFromListRole(List<Role> role)
        {
            List<RoleRequest> listRoleRequest = [];

            foreach (var item in role)
            {
                RoleRequest roleRequest = new()
                {
                    Id = item.RoleId,
                    Title = item.Title ?? string.Empty,
                    Description = item.Description ?? string.Empty
                };
                listRoleRequest.Add(roleRequest);
            }

            return listRoleRequest;
        }
    }
}
