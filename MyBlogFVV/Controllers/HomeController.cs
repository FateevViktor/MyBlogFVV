using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlogFVV.Models;
using System.Diagnostics;

namespace MyBlogFVV.Controllers
{
    [Authorize(Policy = "OnlyForRoleAdmin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
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
    }
}
