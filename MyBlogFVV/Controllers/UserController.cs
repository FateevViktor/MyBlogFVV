using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Interfaces;
using MyBlogFVV.BLL.Models.User;
using MyBlogFVV.WEB.Models.User;
using MyBlogFVV.WEB.Services;
using System.Security.Claims;

namespace MyBlogFVV.WEB.Controllers
{
    [Route("User")]
    [Authorize]
    //[Authorize(Policy = "OnlyForRoleAdmin")]
    //[Authorize(Policy = "OnlyForRoleUser")]
    //[Authorize(Policy = "OnlyForRoleModerator")]
    public class UserController(IUserService userService, IHttpContextAccessor httpContextAccessor, ILogger<UserController> logger) : Controller
    {
        readonly IUserService _userService = userService;
        readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger<UserController> _logger = logger;

        public IActionResult Index()
        {
            return View();
        }
        //---------------Блок регистрации-----------------
        [AllowAnonymous]
        [Route("Register")]
        [HttpGet]
        public IActionResult Register()
        {
            _logger.LogInformation("Пользователь {UserLogin} зашел на страничку Register", User.Identity?.Name);
            ViewBag.RegistrationSuccessful = false;
            return View();
        }
        [AllowAnonymous]
        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ViewBag.RegistrationSuccessful = false;
            _logger.LogInformation("Пользователь {UserLogin} пытается зарегистрироваться", User.Identity?.Name);
            if (ModelState.IsValid)
            {
                try
                {
                    RegisterRequest registerRequest = MyMapping.GetRegisterRequestFromRegisterViewModel(model);
                    _logger.LogInformation("Пытаемся записать пользователя {UserLogin} в БД", model.Login);
                    OperationDetails result = await _userService.Create(registerRequest);
                    if (result.Succedeed == true)
                    {
                        _logger.LogInformation("Пользователь {UserLogin} успешно занесен в БД", model.Login);
                        ViewBag.RegistrationSuccessful = true;
                        return View("Register", model);
                    }
                    else
                    {
                        _logger.LogInformation("Пользователя {UserLogin} не удалось занести в БД, по причине {Message}", model.Login, result.Message);
                        ModelState.AddModelError(result.Property, result.Message);
                        return View("Register", model);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Register класс UserController.");
                    return View("Error");
                }

            }
            else
            {
                _logger.LogInformation("Пользователь {UserLogin} ввел некорректные данные при регистрации", User.Identity?.Name);
                ModelState.AddModelError("", "Некорректные данные");
                return View("Register", model);
            }            
        }
        //------------------------------------------------
        //-------------Вход в учетную запись--------------
        [AllowAnonymous]
        [Route("Login")]
        [HttpGet]
        public IActionResult Login()
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается войти в свою учетную запись", User.Identity?.Name);
            return View();
        }
        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserRequest? userCheck = await _userService.Authenticate(model.Login, model.Password);
                    if (userCheck is null)
                    {
                        ModelState.AddModelError("Login", "Неверно указан логин или пароль");
                        ModelState.AddModelError("Password", "Неверно указан логин или пароль");
                        _logger.LogInformation("Пользователь {UserLogin}, пытаясь войти в свою учетную запись, неверно указал логин или пароль", User.Identity?.Name);
                        return View("Login", model);
                    }
                    //производится установка аутентификационных кук, которые будут применяться для определения клиента и его прав в приложении
                    var claims = new List<Claim>();

                    _logger.LogInformation("Пользователь {UserLogin} вошел в свою учетную запись.", User.Identity?.Name);
                    _logger.LogInformation("Пытаемся записать идентификационные куки");
                    claims.Add(new Claim(ClaimTypes.Name, userCheck.Login));
                    foreach (string role in userCheck.Roles)
                    {
                        if (role != null)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }
                        else
                        {
                            _logger.LogWarning("У пользователя {UserLogin}, почемуто нет роли!", userCheck.Login);
                        }
                    }

                    // создаем объект ClaimsIdentity
                    ClaimsIdentity claimsIdentity = new(claims, "Cookies");
                    // установка аутентификационных куки
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Login класс UserController.");
                    return View("Error");
                }   
            }
            else
            {
                _logger.LogInformation("Пользователь {UserLogin}, пытаясь войти в свою учетную запись, ввел некорректные данные", User.Identity?.Name);
                ModelState.AddModelError("", "Некорректные данные");
                return View("Login", model);
            }
        }
        //------------------------------------------------
        //--------Выход из учетной записи-----------------
        [Route("Logout")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("Пользователь {UserLogin} выходит из своей учетной записи.", User.Identity?.Name);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        //------------------------------------------------
        //--------Моя страничка---------------------------
        [Route("MyPage")]
        [HttpGet]
        public async Task<IActionResult> MyPage()
        {
            MyPageViewModel myPageViewModel;
            _logger.LogInformation("Пользователь {UserLogin} вошел на страничку своего профиля MyPage.", User.Identity?.Name);
            //Мой логин
            string? username = User.Identity?.Name;
            UserRequest? userRequest;
            if (username != null)
            {
                _logger.LogInformation("Запрашиваем личную информацию из БД по пользователю {UserLogin}.", User.Identity?.Name);
                try
                {
                    userRequest = await _userService.GetUserByLogin(username);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе MyPage класс UserController. username={UserLogin}", username);
                    return View("Error");
                }
                if (userRequest != null)
                {
                    _logger.LogInformation("Пользователь {UserLogin} найден в БД.", User.Identity?.Name);
                    myPageViewModel = MyMapping.GetMyPageViewModelFromUserRequest(userRequest);
                    return View(myPageViewModel);
                }
                else
                {
                    _logger.LogWarning("Пользователь {UserLogin} не найден в БД.", User.Identity?.Name);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _logger.LogWarning("При входе пользователя {UserLogin} на страничку своего профиля MyPage, почемуто User.Identity?.Name равен null.", User.Identity?.Name);
                return RedirectToAction("Index", "Home");
            }
        }
        //------------------------------------------------
        //--------Редактирование своей учетной записи-----
        [Route("Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            _logger.LogInformation("Пользователь {UserLogin} вошел на страничку редактирования своего профиля Edit.", User.Identity?.Name);
            UserEditViewModel userEditViewModel;
            //Мой логин
            string? username = User.Identity?.Name;
            UserRequest? userRequest;
            if (username != null)
            {
                _logger.LogInformation("Запрашиваем личную информацию из БД по пользователю {UserLogin}.", User.Identity?.Name);
                try
                {
                    userRequest = await _userService.GetUserByLogin(username);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Edit класс UserController. username={UserLogin}", username);
                    return View("Error");
                }                
                if (userRequest != null)
                {
                    _logger.LogInformation("Пользователь {UserLogin} найден в БД.", User.Identity?.Name);
                    userEditViewModel = MyMapping.GetUserEditViewModelFromUserRequest(userRequest);
                    return View(userEditViewModel);
                }
                else
                {
                    _logger.LogWarning("Пользователь {UserLogin} не найден в БД.", User.Identity?.Name);
                }
                return RedirectToAction("MyPage", "User");
            }
            else
            {
                _logger.LogWarning("При входе пользователя {UserLogin} на страничку редактирования своего профиля Edit, почемуто User.Identity?.Name равен null.", User.Identity?.Name);
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается редактировать свои данные", User.Identity?.Name);
            if (ModelState.IsValid)
            {
                UserEditRequest? userEditRequest;
                if (model != null)
                {
                    userEditRequest = MyMapping.GetUserEditRequestFromUserEditViewModel(model);
                    _logger.LogInformation("Делаем запрос в БД на изменение данных, пользователь {UserLogin}", User.Identity?.Name);
                    OperationDetails result;
                    try
                    {
                        result = await _userService.Update(userEditRequest);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Произошла ошибка в методе Edit класс UserController.");
                        return View("Error");
                    }

                    
                    if (result.Succedeed == true)
                    {
                        ViewBag.EditSuccessful = true;
                        if(result.Property != "Logout")
                        {
                            return RedirectToAction("MyPage", "User");
                        }
                        else
                        {
                            _logger.LogInformation("Пользователь {UserLogin} изменил логин на {UserLogin} - необходимо выйти из профиля.", User.Identity?.Name, model.Login);
                            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                            return RedirectToAction("Index", "Home");
                        }                            
                    }
                    else
                    {
                        _logger.LogInformation("Ошибка запроса в БД на изменение данных ({Message}), пользователь {UserLogin}", User.Identity?.Name, result.Message);
                        ModelState.AddModelError(result.Property, result.Message);
                        return View("Edit", model);
                    }
                }
                return View("Edit", model);
            }
            else
            {
                _logger.LogInformation("Пользователь {UserLogin} ввел некорректные данные", User.Identity?.Name);
                ModelState.AddModelError("", "Некорректные данные");
                return View("Edit", model);
            }
        }
        //------------------------------------------------
        //--------------Удаление пользователя-------------
        [Authorize(Policy = "OnlyForRoleAdmin")]
        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается удалить пользователя с id={id}", User.Identity?.Name, id.ToString());
            if (id>0)
            {
                OperationDetails result;
                try
                {
                    result = await _userService.Delete(id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе Delete класс UserController. id={id}", id.ToString());
                    return View("Error");
                }
                
                if (result.Succedeed == true)
                {
                    _logger.LogInformation("Удаление из БД прошло успешно");
                    return RedirectToAction("AuthorList", "User");
                }
                else
                {
                    _logger.LogInformation("Не получилось удалить из БД. Сообщение - {Message}", result.Message);
                    ModelState.AddModelError(result.Property, result.Message);
                }
            }
            else
            {
                _logger.LogInformation("id удаляемого не больше 0");
                ModelState.AddModelError("Id", "Id должен быть более нуля");
            }
            return RedirectToAction("AuthorList", "User");
        }
        //------------------------------------------------
        //---------Список авторов-------------------
        [Route("AuthorList")]
        [HttpGet]
        public async Task<IActionResult> AuthorList()
        {
            _logger.LogInformation("Пользователь {UserLogin} пытается посмотреть всех авторов AuthorList", User.Identity?.Name);
            SearchUsersViewModel searchUsersViewModel = new();
            List<UserRequest>? UserList;

            try
            {
                UserList = await _userService.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка в методе AuthorList класс UserController.");
                return View("Error");
            }

            if (UserList != null)
            {                     
                searchUsersViewModel = MyMapping.GetSearchUsersViewModelFromListUserRequest(UserList);
            }
            else
            {
                _logger.LogWarning("не удалось загрузить из БД всех авторов");
            }
            return View("AuthorList", searchUsersViewModel);
        }
        [Route("AuthorList")]
        [HttpPost]
        public async Task<IActionResult> AuthorList(int Id)
        {
            SearchUsersViewModel searchUsersViewModel;
            _logger.LogInformation("Пользователь {UserLogin} пытается найти автора по id={id}", User.Identity?.Name, Id.ToString());
            if (Id>0)
            {
                UserRequest? userRequest;
                _logger.LogInformation("Запрашиваем личную информацию из БД по Id={id}.", Id.ToString());
                
                try
                {
                    userRequest = await _userService.GetUserById(Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе AuthorList класс UserController. Id={id}", Id.ToString());
                    return View("Error");
                }                

                List<UserRequest> userList = [];
                if (userRequest != null)
                {
                    _logger.LogInformation("В БД пользователь с Id={id} найден.", Id.ToString());
                    userList.Add(userRequest);
                    searchUsersViewModel = MyMapping.GetSearchUsersViewModelFromListUserRequest(userList);
                    return View("AuthorList", searchUsersViewModel);
                }
                else
                {
                    _logger.LogInformation("В БД, пользователя с Id={id} нет.", Id.ToString());
                    return RedirectToAction("AuthorList");
                }
            }
            else
            {
                _logger.LogInformation("Id не более 0");
                return RedirectToAction("AuthorList");
            }
        }
        //------------------------------------------------
        //--------Моя страничка---------------------------
        [Route("AuthorPage")]
        [HttpGet]
        public async Task<IActionResult> AuthorPage(int id)
        {
            UserRequest? userRequest;
            _logger.LogInformation("Пользователь {UserLogin} пытается посмотреть автора статей c id={id} AuthorPage", User.Identity?.Name, id.ToString());
            AuthorPageViewModel authorPageViewModel;
            if (id>0)
            {
                _logger.LogInformation("Запрашиваем личную информацию из БД по Id={id}.", id.ToString());
                
                try
                {
                    userRequest = await _userService.GetUserById(id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Произошла ошибка в методе AuthorPage класс UserController. id={id}", id.ToString());
                    return View("Error");
                }                

                if (userRequest != null)
                {
                    _logger.LogInformation("Пользователь найден.");
                    authorPageViewModel = MyMapping.GetAuthorPageViewModelFromUserRequest(userRequest);
                    return View("AuthorPage", authorPageViewModel);
                }
                else
                {
                    _logger.LogInformation("Не удалось найти в БД пользователя с id={id}.", id.ToString());
                }
            }
            else
            {
                _logger.LogInformation("id не более 0");
            }
            return RedirectToAction("AuthorList");
        }
    }
}
