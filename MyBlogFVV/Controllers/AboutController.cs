using Microsoft.AspNetCore.Mvc;

namespace MyBlogFVV.WEB.Controllers
{
    public class AboutController(ILogger<AboutController> logger) : Controller
    {
        private readonly ILogger<AboutController> _logger = logger;
        public IActionResult Index()
        {
            _logger.LogInformation("Пользователь {UserLogin} зашел на страничку About", User.Identity?.Name);
            /*
            Для проверки создал исключение
            try
            {
                throw new Exception("Test exception");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка в методе Index класс AboutController");
                //return View("/Home/Error");
                return RedirectToAction("Error", "Home");
            }
            */
           
            return View();
        }
    }
}
