using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MyBlogFVV.API.Models.User;
using MyBlogFVV.API.Services;
using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Interfaces;
using MyBlogFVV.BLL.Models.User;

namespace MyBlogFVV.API.Controllers
{
    /// <summary>
    /// api по упревлению пользователем
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ApiUserController(IUserService userService, ILogger<ApiUserController> logger) : ControllerBase
    {
        readonly IUserService _userService = userService;
        private readonly ILogger<ApiUserController> _logger = logger;

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Сообщение об успешности или нет</returns>
        /// <response code="201">Пользователь успешно зарегистрирован</response>
        /// <response code="400">Возникла ошибка</response>
        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            //ViewBag.RegistrationSuccessful = false;
            _logger.LogInformation("Пользователь {UserLogin} пытается зарегистрироваться", User.Identity?.Name);
            if (ModelState.IsValid)
            {
                try
                {
                    RegisterRequest registerRequest = MyMapping.GetRegisterRequestFromRegisterModel(model);
                    _logger.LogInformation("Пытаемся записать пользователя {UserLogin} в БД", model.Login);
                    OperationDetails result = await _userService.Create(registerRequest);
                    if (result.Succedeed == true)
                    {
                        _logger.LogInformation("Пользователь {UserLogin} успешно занесен в БД", model.Login);
                        //ViewBag.RegistrationSuccessful = true;
                        return StatusCode(201, $"Пользователь {model.Login} зарегистрирован!");
                     }
                    else
                    {
                        _logger.LogInformation("Пользователя {UserLogin} не удалось занести в БД, по причине {Message}", model.Login, result.Message);
                        ModelState.AddModelError(result.Property, result.Message);
                        return StatusCode(400, $"Ошибка: Пользователя {model.Login} не удалось занести в БД, по причине {result.Message}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Register класс UserController.");
                    return StatusCode(400, $"Ошибка: Произошла ошибка в методе Register класс UserController.");
                }

            }
            else
            {
                _logger.LogInformation("Пользователь {UserLogin} ввел некорректные данные при регистрации", User.Identity?.Name);
                ModelState.AddModelError("", "Некорректные данные");
                return StatusCode(400, $"Ошибка: Пользователь {User.Identity?.Name} ввел некорректные данные при регистрации");
            }
        }

        /// <summary>
        /// Список всех пользователей
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Пользователи успешно загрузились</response>
        /// <response code="400">Возникла ошибка при обращении к БД</response>        
        [Route("AuthorList")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AuthorList()
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается посмотреть всех авторов AuthorList", User.Identity?.Name);
            SearchUsersModel searchUsersViewModel;
            List<UserRequest>? UserList;

            try
            {
                UserList = await _userService.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка в методе AuthorList класс UserController.");
                return StatusCode(400, $"Произошла ошибка при обращении к БД");
            }

            if (UserList != null)
            {
                searchUsersViewModel = MyMapping.GetSearchUsersModelFromListUserRequest(UserList);
                return StatusCode(200, searchUsersViewModel);
            }
            else
            {
                _logger.LogWarning("не удалось загрузить из БД всех авторов");
                return StatusCode(400, $"Пользователи не загрузились из БД");
            }
        }

        /// <summary>
        /// Редактирование пользователя
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Пользователя успешно отредактировали</response>
        /// <response code="400">Возникла ошибка</response>  
        [HttpPatch]
        [Route("Edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Edit(
            [FromBody] UserEditModel model)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается редактировать свои данные", User.Identity?.Name);
            if (ModelState.IsValid)
            {
                UserEditRequest? userEditRequest;
                if (model != null)
                {
                    userEditRequest = MyMapping.GetUserEditRequestFromUserEditModel(model);
                    _logger.LogInformation("Делаем запрос в БД на изменение данных, пользователь {UserLogin}", User.Identity?.Name);
                    OperationDetails result;
                    try
                    {
                        result = await _userService.Update(userEditRequest);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Произошла ошибка в методе Edit класс UserController.");
                        return StatusCode(400, $"Произошла ошибка при обращении к БД");
                    }


                    if (result.Succedeed == true)
                    {
                        if (result.Property != "Logout")
                        {
                            return StatusCode(200, "Редактирование пользователя прошло успешно");
                        }
                        else
                        {
                            _logger.LogInformation("Пользователь {UserLogin} изменил логин на {UserLogin} - необходимо выйти из профиля.", User.Identity?.Name, model.Login);
                            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                            return StatusCode(200, "Редактирование пользователя прошло успешно, необходимо заново войти в учетную запись");
                        }
                    }
                    else
                    {
                        _logger.LogInformation("Ошибка запроса в БД на изменение данных ({Message}), пользователь {UserLogin}", User.Identity?.Name, result.Message);
                        ModelState.AddModelError(result.Property, result.Message);
                        return StatusCode(400, $"Произошла ошибка при обращении к БД");
                    }
                }
                else
                {
                    return StatusCode(400, $"Ошибка: Передана пустая модель");
                }
            }
            else
            {
                _logger.LogInformation("Пользователь {UserLogin} ввел некорректные данные", User.Identity?.Name);
                ModelState.AddModelError("", "Некорректные данные");
                return StatusCode(400, $"Ошибка: Пользователь {User.Identity?.Name} ввел некорректные данные при регистрации");
            }
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Пользователя успешно удалили</response>
        /// <response code="400">Возникла ошибка</response>
        [HttpDelete]
        [Route("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается удалить пользователя с id={id}", User.Identity?.Name, id.ToString());
            if (id > 0)
            {
                OperationDetails result;
                try
                {
                    result = await _userService.Delete(id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Delete класс UserController. id={id}", id.ToString());
                    return StatusCode(400, $"Ошибка: Произошла ошибка при обращении к БД");
                }

                if (result.Succedeed == true)
                {
                    _logger.LogInformation("Удаление из БД прошло успешно");
                    return StatusCode(200, $"Пользователя удалили");
                }
                else
                {
                    _logger.LogInformation("Не получилось удалить из БД. Сообщение - {Message}", result.Message);
                    ModelState.AddModelError(result.Property, result.Message);
                    return StatusCode(400, $"Ошибка: {result.Message}");
                }
            }
            else
            {
                _logger.LogInformation("id удаляемого не больше 0");
                ModelState.AddModelError("Id", "Id должен быть более нуля");
                return StatusCode(400, $"Ошибка: Id должен быть более нуля");
            }
        }
    }
}
