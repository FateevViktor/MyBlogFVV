using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Interfaces;
using MyBlogFVV.BLL.Models.Role;
using MyBlogFVV.DAL.Entities;
using MyBlogFVV.WEB.Models.Role;
using MyBlogFVV.WEB.Services;

namespace MyBlogFVV.WEB.Controllers
{
    [Route("Role")]
    [Authorize]
    public class RoleController(IRoleService roleService, ILogger<RoleController> logger) : Controller
    {
        readonly IRoleService _roleService = roleService;
        private readonly ILogger<RoleController> _logger = logger;

        public IActionResult Index()
        {
            return View();
        }
        //---------------Блок создания тега-----------------
        [Route("Add")]
        [HttpGet]
        public IActionResult Add()
        {
            _logger.LogInformation("Пользователь {UserLogin} зашел на страничку создания роли. Add", User.Identity?.Name);
            RoleViewModel roleViewModel = new();
            return View(roleViewModel);
        }
        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> Add(RoleViewModel model)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается создать роль. Add", User.Identity?.Name);
            if (ModelState.IsValid)
            {
                RoleRequest roleRequest = MyMappingRole.GetRoleRequestFromRoleViewModel(model);
                _logger.LogInformation("Запрос в БД. Посылаем запрос на создания роли в БД. Add");

                OperationDetails result;
                try
                {
                    result = await _roleService.Create(roleRequest);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Add класс RoleController.");
                    return View("Error");
                }

                if (result.Succedeed == true)
                {
                    _logger.LogInformation("Роль добавили. Add");
                    return RedirectToAction("RoleList");
                }
                else
                {
                    _logger.LogWarning("Добавить роль в БД не удалось. Событие - {Message}. Add", result.Message);
                    ModelState.AddModelError(result.Property, result.Message);
                    return View("Add", model);
                }
            }
            else
            {
                _logger.LogInformation("Введены некорректные данные. Add");
                ModelState.AddModelError("", "Некорректные данные");
                return View("Add", model);
            }            
        }
        //------------------------------------------------
        //--------Редактирование роли-------------------
        [Route("Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(int roleId)
        {
            _logger.LogInformation("Пользователь {UserLogin} зашел в роль с id={id}. Edit", User.Identity?.Name, roleId.ToString());
            RoleViewModel roleViewModel;
            RoleRequest? roleRequest;
            if (roleId > 0)
            {
                _logger.LogInformation("Запрос в БД. Посылаем запрос на получение роли по её id={id}. Edit", roleId.ToString());

                try
                {
                    roleRequest = await _roleService.GetRoleById(roleId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Edit класс RoleController. roleId={id}", roleId.ToString());
                    return View("Error");
                }                

                if (roleRequest != null)
                {
                    _logger.LogInformation("Подгрузили. Edit");
                    roleViewModel = MyMappingRole.GetRoleViewModelFromRoleRequest(roleRequest);
                    return View("Edit", roleViewModel);
                }
                else
                {
                    _logger.LogInformation("Не подгрузили. Edit");
                }
            }
            else
            {
                _logger.LogInformation("id равен нулю. Edit");
            }
            return RedirectToAction("RoleList");
        }

        [Route("UpdateEdit")]
        [HttpPost]
        public async Task<IActionResult> UpdateEdit(RoleViewModel model)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается изменить роль. UpdateEdit", User.Identity?.Name);
            RoleRequest? roleRequest;
            if (ModelState.IsValid)
            {
                roleRequest = MyMappingRole.GetRoleRequestFromRoleViewModel(model);
                _logger.LogInformation("Запрос в БД. Посылаем запрос на изменение роли. UpdateEdit");

                OperationDetails result;
                try
                {
                    result = await _roleService.Update(roleRequest);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе UpdateEdit класс RoleController.}");
                    return View("Error");
                }

                if (result.Succedeed == true)
                {
                    _logger.LogInformation("Получилось. UpdateEdit");
                    return RedirectToAction("RoleList");
                }
                else
                {
                    _logger.LogWarning("Не удалось. Сообщение - {Message}. UpdateEdit", result.Message);
                    ModelState.AddModelError(result.Property, result.Message);
                    return View("Edit", model);
                }
            }
            else
            {
                _logger.LogInformation("Введены некорректные данные. UpdateEdit");
                ModelState.AddModelError("", "Некорректные данные");
                return View("Edit", model);
            }
        }
        //------------------------------------------------
        //--------------Удаление роли-------------
        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int roleId)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается удалить роль. Delete", User.Identity?.Name);
            if (roleId > 0)
            {
                _logger.LogInformation("Запрос в БД. Посылаем запрос на удаление роли. Delete");

                OperationDetails result;
                try
                {
                    result = await _roleService.Delete(roleId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Delete класс RoleController. roleId={id}", roleId.ToString());
                    return View("Error");
                }

                if (result.Succedeed == true)
                {
                    _logger.LogInformation("Получилось. Delete");
                    return RedirectToAction("RoleList");
                }
                else
                {
                    _logger.LogWarning("Удалить не удалось. Сообщение - {Message}. Delete", result.Message);
                    ModelState.AddModelError(result.Property, result.Message);
                    return RedirectToAction("RoleList");
                }
            }
            else
            {
                _logger.LogInformation("id равен нулю. Delete");
                ModelState.AddModelError("Id", "Id должен быть более нуля");
            }
            return RedirectToAction("RoleList");
        }
        //------------------------------------------------
        //---------Список всех ролей-------------------
        [Route("RoleList")]
        [HttpGet]
        public async Task<IActionResult> RoleList()
        {
            _logger.LogInformation("Пользователь {UserLogin} зашел на страничку просмотра ролей. RoleList", User.Identity?.Name);
            SearchRolesViewModel searchRolesViewModel = new();
            _logger.LogInformation("Запрос в БД. Посылаем запрос на подгрузку всех ролей. RoleList");

            List<RoleRequest>? roleList;
            try
            {
                roleList = await _roleService.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка в методе RoleList класс RoleController.");
                return View("Error");
            }

            if (roleList != null)
            {
                _logger.LogInformation("Подгрузили. RoleList");
                searchRolesViewModel = MyMappingRole.GetSearchRolesViewModelFromListRoleRequest(roleList);
            }
            else
            {
                _logger.LogInformation("Не удалось. RoleList");
            }
            return View("RoleList", searchRolesViewModel);
        }

        [Route("RoleList")]
        [HttpPost]
        public async Task<IActionResult> RoleList(int Id)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается найти роль по ее id={id}. RoleList", User.Identity?.Name, Id.ToString());
            SearchRolesViewModel searchRolesViewModel;
            if (Id > 0)
            {
                RoleRequest? roleRequest;
                _logger.LogInformation("Запрос в БД. Посылаем запрос на подгрузку роли по её id={id}. RoleList", Id.ToString());

                try
                {
                    roleRequest = await _roleService.GetRoleById(Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе RoleList класс RoleController. id={id}", Id.ToString());
                    return View("Error");
                }
                
                List<RoleRequest> roleList = [];
                if (roleRequest != null)
                {
                    _logger.LogInformation("Подгрузили. RoleList");
                    roleList.Add(roleRequest);
                    searchRolesViewModel = MyMappingRole.GetSearchRolesViewModelFromListRoleRequest(roleList);
                    return View("RoleList", searchRolesViewModel);
                }
                else
                {
                    _logger.LogWarning("Подгрузить не удалось. RoleList");
                    return RedirectToAction("RoleList");
                }
            }
            else
            {
                _logger.LogInformation("id равен нулю. RoleList");
                return RedirectToAction("RoleList");
            }
        }
        //------------------------------------------------
        //------------Страничка просмотра роли------------
        [Route("ShowRole")]
        [HttpPost]
        public async Task<IActionResult> ShowRole(int roleId)
        {
            _logger.LogInformation("Пользователь {UserLogin} зашел на страничку просмотра роли. ShowRole", User.Identity?.Name);
            RoleViewModel roleViewModel;
            if (roleId > 0)
            {
                RoleRequest? roleRequest;
                _logger.LogInformation("Запрос в БД. Посылаем запрос на подгрузку роли по её id={id}. ShowRole", roleId.ToString());

                try
                {
                    roleRequest = await _roleService.GetRoleById(roleId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе ShowRole класс RoleController. roleId={id}", roleId.ToString());
                    return View("Error");
                }
                
                if (roleRequest != null)
                {
                    _logger.LogInformation("Подгрузили. ShowRole");
                    roleViewModel = MyMappingRole.GetRoleViewModelFromRoleRequest(roleRequest);
                    return View("ShowRole", roleViewModel);
                }
                else
                {
                    _logger.LogWarning("Не удалось. ShowRole");
                }
            }
            else
            {
                _logger.LogInformation("id равен нулю. ShowRole");
            }
            return RedirectToAction("RoleList", "Role");
        }
        //------------------------------------------------
    }
}
