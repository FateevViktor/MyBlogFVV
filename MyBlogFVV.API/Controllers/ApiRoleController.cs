using Microsoft.AspNetCore.Mvc;
using MyBlogFVV.API.Models.Role;
using MyBlogFVV.API.Services;
using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Interfaces;
using MyBlogFVV.BLL.Models.Role;

namespace MyBlogFVV.API.Controllers
{
    /// <summary>
    /// api по упревлению ролями
    /// </summary>
    /// <returns></returns>
    [ApiController]
    [Route("api/[controller]")]
    public class ApiRoleController(IRoleService roleService, ILogger<ApiRoleController> logger) : ControllerBase
    {
        readonly IRoleService _roleService = roleService;
        private readonly ILogger<ApiRoleController> _logger = logger;

        /// <summary>
        /// Добавление новой роли
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Сообщение об успешности или нет</returns>
        /// <response code="201">Роль успешно добавлена</response>
        /// <response code="400">Возникла ошибка</response>
        [Route("Add")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] RoleModel model)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается создать роль. Add", User.Identity?.Name);
            if (ModelState.IsValid)
            {
                RoleRequest roleRequest = MyMappingRole.GetRoleRequestFromRoleModel(model);
                _logger.LogInformation("Запрос в БД. Посылаем запрос на создания роли в БД. Add");

                OperationDetails result;
                try
                {
                    result = await _roleService.Create(roleRequest);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Add класс RoleController.");
                    return StatusCode(400, $"Ошибка: Роль не удалось занести в БД");
                }

                if (result.Succedeed == true)
                {
                    _logger.LogInformation("Роль добавили. Add");
                    return StatusCode(201, $"Роль успешно добавлена!");
                }
                else
                {
                    _logger.LogWarning("Добавить роль в БД не удалось. Событие - {Message}. Add", result.Message);
                    ModelState.AddModelError(result.Property, result.Message);
                    return StatusCode(400, $"Ошибка: Добавить роль в БД не удалось. Событие - {result.Message}");
                }
            }
            else
            {
                _logger.LogInformation("Введены некорректные данные. Add");
                ModelState.AddModelError("", "Некорректные данные");
                return StatusCode(400, $"Ошибка: Введены некорректные данные");
            }
        }

        /// <summary>
        /// Редактирование роли
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <response code="200">Роль успешно изменена</response>
        /// <response code="400">Возникла ошибка</response>
        [Route("Edit")]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Edit([FromBody] RoleModel model)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается изменить роль. Edit", User.Identity?.Name);
            RoleRequest? roleRequest;
            if (ModelState.IsValid)
            {
                roleRequest = MyMappingRole.GetRoleRequestFromRoleModel(model);
                _logger.LogInformation("Запрос в БД. Посылаем запрос на изменение роли. Edit");

                OperationDetails result;
                try
                {
                    result = await _roleService.Update(roleRequest);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Edit класс RoleController.}");
                    return StatusCode(400, $"Ошибка: Роль не удалось изменить в БД");
                }

                if (result.Succedeed == true)
                {
                    _logger.LogInformation("Получилось. Edit");
                    return StatusCode(200, $"Роль успешно изменена!");
                }
                else
                {
                    _logger.LogWarning("Не удалось. Сообщение - {Message}. UpdateEdit", result.Message);
                    ModelState.AddModelError(result.Property, result.Message);
                    return StatusCode(400, $"Ошибка: Роль не удалось изменить - {result.Message}");
                }
            }
            else
            {
                _logger.LogInformation("Введены некорректные данные. Edit");
                ModelState.AddModelError("", "Некорректные данные");
                return StatusCode(400, $"Ошибка: Введены некорректные данные");
            }
        }

        /// <summary>
        /// Удаление роли
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        /// <response code="200">Роль успешно удалили</response>
        /// <response code="400">Возникла ошибка</response>
        [HttpDelete]
        [Route("Delete/{roleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] int roleId)
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
                    return StatusCode(400, $"Ошибка: Роль не удалось удалить из БД");
                }

                if (result.Succedeed == true)
                {
                    _logger.LogInformation("Получилось. Delete");
                    return StatusCode(200, $"Роль успешно удалили!");
                }
                else
                {
                    _logger.LogWarning("Удалить не удалось. Сообщение - {Message}. Delete", result.Message);
                    ModelState.AddModelError(result.Property, result.Message);
                    return StatusCode(400, $"Ошибка: Удалить не удалось. Сообщение - {result.Message}");
                }
            }
            else
            {
                _logger.LogInformation("id равен нулю. Delete");
                ModelState.AddModelError("Id", "Id должен быть более нуля");
                return StatusCode(400, $"Ошибка: Id должен быть более нуля");
            }
        }

        /// <summary>
        /// Посмотреть все роли
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Успешно загрузили роли</response>
        /// <response code="400">Возникла ошибка</response>
        [Route("RoleList")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RoleList()
        {
            _logger.LogInformation("Пользователь {UserLogin} зашел на страничку просмотра ролей. RoleList", User.Identity?.Name);
            SearchRolesModel searchRolesViewModel;
            _logger.LogInformation("Запрос в БД. Посылаем запрос на подгрузку всех ролей. RoleList");

            List<RoleRequest>? roleList;
            try
            {
                roleList = await _roleService.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка в методе RoleList класс RoleController.");
                return StatusCode(400, $"Ошибка: Роль не удалось загрузить из БД");
            }

            if (roleList != null)
            {
                _logger.LogInformation("Подгрузили. RoleList");
                searchRolesViewModel = MyMappingRole.GetSearchRolesModelFromListRoleRequest(roleList);
                return StatusCode(200, searchRolesViewModel);
            }
            else
            {
                _logger.LogInformation("Не удалось. RoleList");
                return StatusCode(400, $"Ошибка: Загрузить не удалось.");
            }
        }
    }
}
