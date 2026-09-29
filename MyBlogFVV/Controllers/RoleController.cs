using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Interfaces;
using MyBlogFVV.BLL.Models.Role;
using MyBlogFVV.WEB.Models.Role;
using MyBlogFVV.WEB.Services;

namespace MyBlogFVV.WEB.Controllers
{
    [Route("Role")]
    [Authorize]
    public class RoleController(IRoleService roleService) : Controller
    {
        readonly IRoleService _roleService = roleService;

        public IActionResult Index()
        {
            return View();
        }
        //---------------Блок создания тега-----------------
        [Route("Add")]
        [HttpGet]
        public IActionResult Add()
        {
            RoleViewModel roleViewModel = new();
            return View(roleViewModel);
        }
        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> Add(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                RoleRequest roleRequest = MyMappingRole.GetRoleRequestFromRoleViewModel(model);

                OperationDetails result = await _roleService.Create(roleRequest);
                if (result.Succedeed == true)
                {
                    return RedirectToAction("RoleList");
                }
                else
                {
                    ModelState.AddModelError(result.Property, result.Message);
                }
            }
            return RedirectToAction("RoleList");
        }
        //------------------------------------------------
        //--------Редактирование тега-------------------
        [Route("Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(int roleId)
        {
            RoleViewModel roleViewModel;
            RoleRequest? roleRequest;
            if (roleId > 0)
            {
                roleRequest = await _roleService.GetRoleById(roleId);

                if (roleRequest != null)
                {
                    roleViewModel = MyMappingRole.GetRoleViewModelFromRoleRequest(roleRequest);
                    return View("Edit", roleViewModel);
                }
            }
            return RedirectToAction("RoleList");
        }

        [Route("UpdateEdit")]
        [HttpPost]
        public async Task<IActionResult> UpdateEdit(RoleViewModel model)
        {
            RoleRequest? roleRequest;
            if (ModelState.IsValid)
            {
                roleRequest = MyMappingRole.GetRoleRequestFromRoleViewModel(model);
                OperationDetails result = await _roleService.Update(roleRequest);
                if (result.Succedeed == true)
                {
                    return RedirectToAction("RoleList");
                }
                else
                {
                    ModelState.AddModelError(result.Property, result.Message);
                    return RedirectToAction("RoleList");
                }
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return View("Edit", model);
            }
        }
        //------------------------------------------------
        //--------------Удаление тега-------------
        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int roleId)
        {
            if (roleId > 0)
            {
                OperationDetails result = await _roleService.Delete(roleId);

                if (result.Succedeed == true)
                {
                    return RedirectToAction("RoleList");
                }
                else
                {
                    ModelState.AddModelError(result.Property, result.Message);
                    return RedirectToAction("RoleList");
                }
            }
            else
            {
                ModelState.AddModelError("Id", "Id должен быть более нуля");
            }
            return RedirectToAction("RoleList");
        }
        //------------------------------------------------
        //---------Список всех тегов-------------------
        [Route("RoleList")]
        [HttpGet]
        public async Task<IActionResult> RoleList()
        {
            SearchRolesViewModel searchRolesViewModel = new();
            List<RoleRequest>? roleList = await _roleService.GetAll();
            if (roleList != null)
            {
                searchRolesViewModel = MyMappingRole.GetSearchRolesViewModelFromListRoleRequest(roleList);
            }
            return View("RoleList", searchRolesViewModel);
        }

        [Route("RoleList")]
        [HttpPost]
        public async Task<IActionResult> RoleList(int Id)
        {
            SearchRolesViewModel searchRolesViewModel;
            if (Id > 0)
            {
                RoleRequest? roleRequest;
                roleRequest = await _roleService.GetRoleById(Id);
                List<RoleRequest> roleList = [];
                if (roleRequest != null)
                {
                    roleList.Add(roleRequest);
                    searchRolesViewModel = MyMappingRole.GetSearchRolesViewModelFromListRoleRequest(roleList);
                    return View("RoleList", searchRolesViewModel);
                }
                else
                {
                    return RedirectToAction("RoleList");
                }
            }
            else
            {
                return RedirectToAction("RoleList");
            }
        }
        //------------------------------------------------
        //------------Страничка просмотра роли------------
        [Route("ShowRole")]
        [HttpPost]
        public async Task<IActionResult> ShowRole(int roleId)
        {
            RoleViewModel roleViewModel;
            if (roleId > 0)
            {
                RoleRequest? roleRequest;
                roleRequest = await _roleService.GetRoleById(roleId);
                if (roleRequest != null)
                {
                    roleViewModel = MyMappingRole.GetRoleViewModelFromRoleRequest(roleRequest);
                    return View("ShowRole", roleViewModel);
                }
            }
            return RedirectToAction("RoleList", "Role");
        }
        //------------------------------------------------
    }
}
