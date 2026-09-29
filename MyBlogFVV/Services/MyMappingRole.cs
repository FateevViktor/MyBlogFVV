using MyBlogFVV.BLL.Models.Role;
using MyBlogFVV.WEB.Models.Role;

namespace MyBlogFVV.WEB.Services
{
    public class MyMappingRole
    {
        public static RoleRequest GetRoleRequestFromRoleViewModel(RoleViewModel roleViewModel)
        {
            RoleRequest roleRequest = new()
            {
                Id = roleViewModel.Id,
                Title = roleViewModel.Title,
                Description = roleViewModel.Description
            };
            return roleRequest;
        }
        public static RoleViewModel GetRoleViewModelFromRoleRequest(RoleRequest roleRequest)
        {
            RoleViewModel roleViewModel = new()
            {
                Id = roleRequest.Id,
                Title = roleRequest.Title,
                Description = roleRequest.Description
            };
            return roleViewModel;
        }
        public static SearchRolesViewModel GetSearchRolesViewModelFromListRoleRequest(List<RoleRequest> roleRequest)
        {
            SearchRolesViewModel searchRolesViewModel = new();
            foreach (var item in roleRequest)
            {
                RoleViewModel roleViewModel = new()
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
