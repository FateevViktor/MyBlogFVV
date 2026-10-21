using MyBlogFVV.API.Models.Role;
using MyBlogFVV.BLL.Models.Role;

namespace MyBlogFVV.API.Services
{
    public class MyMappingRole
    {
        public static RoleRequest GetRoleRequestFromRoleModel(RoleModel roleViewModel)
        {
            RoleRequest roleRequest = new()
            {
                Id = roleViewModel.Id,
                Title = roleViewModel.Title,
                Description = roleViewModel.Description
            };
            return roleRequest;
        }
        public static RoleModel GetRoleModelFromRoleRequest(RoleRequest roleRequest)
        {
            RoleModel roleViewModel = new()
            {
                Id = roleRequest.Id,
                Title = roleRequest.Title,
                Description = roleRequest.Description
            };
            return roleViewModel;
        }
        public static SearchRolesModel GetSearchRolesModelFromListRoleRequest(List<RoleRequest> roleRequest)
        {
            SearchRolesModel searchRolesViewModel = new();
            foreach (var item in roleRequest)
            {
                RoleModel roleViewModel = new()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description
                };

                searchRolesViewModel.RoleList.Add(roleViewModel);
            }
            return searchRolesViewModel;
        }
    }
}
