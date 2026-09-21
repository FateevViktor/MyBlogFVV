using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlogFVV.BLL.Infrastructure;
using MyBlogFVV.BLL.Interfaces;
using MyBlogFVV.BLL.Models.User;
using MyBlogFVV.WEB.Models.Comment;
using MyBlogFVV.WEB.Models.Post;
using MyBlogFVV.WEB.Models.Tag;
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
    public class UserController : Controller
    {
        MyMapping myMapping = new MyMapping();
        IUserService _userService;
        IHttpContextAccessor _httpContextAccessor;
        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }
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
                RegisterRequest registerRequest = myMapping.GetRegisterRequestFromRegisterViewModel(model);

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
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
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
            MyPageViewModel myPageViewModel = new MyPageViewModel();
            List<PostViewModel> posts = new List<PostViewModel>();
            List<CommentViewModel> comments = new List<CommentViewModel>();
            List<TagViewModel> tags = new List<TagViewModel>();
            //List<string> roles = new List<string>();
            var s = User;
            //Мой логин
            string? username = _httpContextAccessor.HttpContext.User.Identity.Name;
            var rolesClims = _httpContextAccessor.HttpContext.User.Claims.ToList();
            UserRequest? userRequest = null;
            if (username != null)
            {
                userRequest = await _userService.GetUserByLogin(username);
                if (userRequest != null)
                {
                    myPageViewModel = myMapping.GetMyPageViewModelFromUserRequest(userRequest);
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
            UserEditViewModel userEditViewModel = new UserEditViewModel();
            //Мой логин
            string? username = _httpContextAccessor.HttpContext.User.Identity.Name;
            var rolesClims = _httpContextAccessor.HttpContext.User.Claims.ToList();
            UserRequest? userRequest = null;
            if (username != null)
            {
                userRequest = await _userService.GetUserByLogin(username);
                if (userRequest != null)
                {
                    userEditViewModel = myMapping.GetUserEditViewModelFromUserRequest(userRequest);
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
                UserEditRequest? userEditRequest = null;
                if (model != null)
                {
                    userEditRequest = myMapping.GetUserEditRequestFromUserEditViewModel(model);
                    OperationDetails result = await _userService.Update(userEditRequest);
                    if (result.Succedeed == true)
                    {
                        ViewBag.EditSuccessful = true;
                        return RedirectToAction("MyPage", "User");
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
            SearchUsersViewModel searchUsersViewModel = new SearchUsersViewModel();
            List<UserRequest>? UserList = await _userService.GetAll();
            if (UserList != null)
            {                
                searchUsersViewModel = myMapping.GetSearchUsersViewModelFromListUserRequest(UserList);
            }
            return View("AuthorList", searchUsersViewModel);
        }
        [Route("AuthorList")]
        [HttpPost]
        public async Task<IActionResult> AuthorList(int Id)
        {
            SearchUsersViewModel searchUsersViewModel = new SearchUsersViewModel();
            if (Id>0)
            {
                UserRequest? userRequest = new UserRequest();
                userRequest = await _userService.GetUserById(Id);
                List<UserRequest> userList = new List<UserRequest>();
                if (userRequest != null)
                {
                    userList.Add(userRequest);
                    searchUsersViewModel = myMapping.GetSearchUsersViewModelFromListUserRequest(userList);
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
    }
}
