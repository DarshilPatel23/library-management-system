using Microsoft.AspNetCore.Mvc;
using LMS.Models;

namespace LMS.Controllers
{
    public class IssuedBooksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
