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
    public class UserController(IUserService userService, IHttpContextAccessor httpContextAccessor) : Controller
    {
        readonly IUserService _userService = userService;
        readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

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
            ViewBag.RegistrationSuccessful = false;
            return View();
        }
        [AllowAnonymous]
        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ViewBag.RegistrationSuccessful = false;
            if (ModelState.IsValid)
            {
                RegisterRequest registerRequest = MyMapping.GetRegisterRequestFromRegisterViewModel(model);

                OperationDetails result = await _userService.Create(registerRequest);
                if (result.Succedeed == true)
                {
                    ViewBag.RegistrationSuccessful = true;
                    return View(model);
                    //return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(result.Property, result.Message);
                }
            }
            return View("Register", model);
        }
        //------------------------------------------------
        //-------------Вход в учетную запись--------------
        [AllowAnonymous]
        [Route("Login")]
        [HttpGet]
        public IActionResult Login()
        {
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
                UserRequest? userCheck = await _userService.Authenticate(model.Login, model.Password);
                if (userCheck is null)
                {
                    ModelState.AddModelError("Login", "Неверно указан логин или пароль");
                    ModelState.AddModelError("Password", "Неверно указан логин или пароль");
                    return View("Login", model);
                }
                //производится установка аутентификационных кук, которые будут применяться для определения клиента и его прав в приложении
                /*
                var claims = new List<Claim> 
                { 
                    new Claim(ClaimTypes.Name, userCheck.Login)
                };*/
                var claims = new List<Claim>();
                if(userCheck.Login!=null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, userCheck.Login));
                    foreach (string role in userCheck.Roles)
                    {
                        if (role != null)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }
                    }
                }

                // создаем объект ClaimsIdentity
                ClaimsIdentity claimsIdentity = new(claims, "Cookies");
                // установка аутентификационных куки
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Index", "Home");
            }
            return View("Login", model);
        }
        //------------------------------------------------
        //--------Выход из учетной записи-----------------
        [Route("Logout")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
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
            //Мой логин
            string? username = User.Identity?.Name;
            UserRequest? userRequest;
            if (username != null)
            {
                userRequest = await _userService.GetUserByLogin(username);
                if (userRequest != null)
                {
                    myPageViewModel = MyMapping.GetMyPageViewModelFromUserRequest(userRequest);
                    return View(myPageViewModel);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //------------------------------------------------
        //--------Редактирование своей учетной записи-----
        [Route("Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            UserEditViewModel userEditViewModel;
            //Мой логин
            string? username = User.Identity?.Name;
            UserRequest? userRequest;
            if (username != null)
            {
                userRequest = await _userService.GetUserByLogin(username);
                if (userRequest != null)
                {
                    userEditViewModel = MyMapping.GetUserEditViewModelFromUserRequest(userRequest);
                    return View(userEditViewModel);
                }
                return RedirectToAction("MyPage", "User");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserEditRequest? userEditRequest;
                if (model != null)
                {
                    userEditRequest = MyMapping.GetUserEditRequestFromUserEditViewModel(model);
                    OperationDetails result = await _userService.Update(userEditRequest);
                    if (result.Succedeed == true)
                    {
                        ViewBag.EditSuccessful = true;
                        if(result.Property != "Logout")
                        {
                            return RedirectToAction("MyPage", "User");
                        }
                        else
                        {
                            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                            return RedirectToAction("Index", "Home");
                        }                            
                    }
                    else
                    {
                        ModelState.AddModelError(result.Property, result.Message);
                    }
                }
                return View("Edit", model);
            }
            else
            {
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
            if(id>0)
            {
                OperationDetails result = await _userService.Delete(id);
                if (result.Succedeed == true)
                {
                    return RedirectToAction("AuthorList", "User");
                }
                else
                {
                    ModelState.AddModelError(result.Property, result.Message);
                }
            }
            else
            {
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
            SearchUsersViewModel searchUsersViewModel = new();
            List<UserRequest>? UserList = await _userService.GetAll();
            if (UserList != null)
            {                
                searchUsersViewModel = MyMapping.GetSearchUsersViewModelFromListUserRequest(UserList);
            }
            return View("AuthorList", searchUsersViewModel);
        }
        [Route("AuthorList")]
        [HttpPost]
        public async Task<IActionResult> AuthorList(int Id)
        {
            SearchUsersViewModel searchUsersViewModel;
            if (Id>0)
            {
                UserRequest? userRequest;
                userRequest = await _userService.GetUserById(Id);
                List<UserRequest> userList = [];
                if (userRequest != null)
                {
                    userList.Add(userRequest);
                    searchUsersViewModel = MyMapping.GetSearchUsersViewModelFromListUserRequest(userList);
                    return View("AuthorList", searchUsersViewModel);
                }
                else
                {
                    return RedirectToAction("AuthorList");
                }
            }
            else
            {
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
            AuthorPageViewModel authorPageViewModel;
            if (id>0)
            {                
                userRequest = await _userService.GetUserById(id);
                if (userRequest != null)
                {
                    authorPageViewModel = MyMapping.GetAuthorPageViewModelFromUserRequest(userRequest);
                    return View("AuthorPage", authorPageViewModel);
                }
            }
            return RedirectToAction("AuthorList");
        }
    }
}
