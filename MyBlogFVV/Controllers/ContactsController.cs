using Microsoft.AspNetCore.Mvc;

namespace MyBlogFVV.WEB.Controllers
{
    public class ContactsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
