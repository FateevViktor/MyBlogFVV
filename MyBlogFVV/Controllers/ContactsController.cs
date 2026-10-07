using Microsoft.AspNetCore.Mvc;

namespace MyBlogFVV.WEB.Controllers
{
    public class ContactsController(ILogger<ContactsController> logger) : Controller
    {
        private readonly ILogger<ContactsController> _logger = logger;
        public IActionResult Index()
        {
            _logger.LogInformation("Пользователь {UserLogin} зашел на страничку Contacts", User.Identity?.Name);
            return View();
        }
    }
}
