using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyBlogFVV.BLL.Interfaces;
using MyBlogFVV.BLL.Models.Post;
using MyBlogFVV.WEB.Models.Post;
using MyBlogFVV.WEB.Services;

namespace MyBlogFVV.WEB.Controllers
{
    [Authorize(Policy = "OnlyForRoleAdmin")]
    public class HomeController(ILogger<HomeController> logger, IPostService postService) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        readonly IPostService _postService = postService;

        /// <summary>
        /// Главная страница, выдаем последние посты.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Пользователь {UserLogin} зашел на страничку Home", User.Identity?.Name);
            SearchPostsViewModel searchPostsViewModel = new();
            _logger.LogInformation("Пытаемся загрузить статьи из БД, пользователь {UserLogin}", User.Identity?.Name);

            List<PostRequest> postList;
            try
            {
                postList = await _postService.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка в методе Index класс HomeController.");
                return View("Error");
            }

            List<PostRequest> postListSorted = [];
            postListSorted = [.. postList.OrderByDescending(p => p.PostDate)];
            if (postList != null)
            {
                _logger.LogInformation("Статьи найдены");
                searchPostsViewModel = MyMappingPost.GetSearchPostsViewModelFromListPostRequest(postListSorted);
            }
            else
            {
                _logger.LogWarning("Не удалось найти статьи в БД");
            }
                return View(searchPostsViewModel);
        }
        [AllowAnonymous]
        public IActionResult About()
        {
            _logger.LogInformation("Пользователь {UserLogin} зашел на страничку About", User.Identity?.Name);
            return View();
        }
        [AllowAnonymous]
        public IActionResult Contacts()
        {
            _logger.LogInformation("Пользователь {UserLogin} зашел на страничку Contacts", User.Identity?.Name);
            return View();
        }
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionFeature != null)
            {
                Exception ex = exceptionFeature.Error;
                string path = exceptionFeature.Path; // Путь, где произошла ошибка
                _logger.LogError(/*ex, */"Произошла непредвиденная ошибка. Путь {Path}. Сообщение {ErrorMessage}", path, ex.Message);
            }
            return View();
        }
        /// <summary>
        /// страничка выпадает, если пользователь делает запрещенное действие, не в рамках его роли
        /// </summary>
        /// <returns></returns>
        [Route("/AccessDanied")]
        [AllowAnonymous]
        public IActionResult AccessDanied()
        {
            return View();
        }

        /// <summary>
        /// страничка выпадает, если пользователь переходит на несуществующую страничку
        /// </summary>
        /// <returns></returns>
        [Route("NotFoundContent")]
        [AllowAnonymous]
        public IActionResult NotFoundContent()
        {
            return View();
        }
    }
}
