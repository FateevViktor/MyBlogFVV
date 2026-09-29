using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlogFVV.BLL.Interfaces;
using MyBlogFVV.BLL.Models.Post;
using MyBlogFVV.WEB.Models;
using MyBlogFVV.WEB.Models.Post;
using MyBlogFVV.WEB.Services;
using System.Diagnostics;

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
            SearchPostsViewModel searchPostsViewModel = new();

            List<PostRequest> postList = await _postService.GetAll();
            List<PostRequest> postListSorted = [];
            postListSorted = [.. postList.OrderByDescending(p => p.PostDate)];
            if (postList != null)
            {
                searchPostsViewModel = MyMappingPost.GetSearchPostsViewModelFromListPostRequest(postListSorted);
            }
            return View(searchPostsViewModel);
        }
        [AllowAnonymous]
        public IActionResult About()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Contacts()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
