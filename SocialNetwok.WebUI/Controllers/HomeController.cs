using Microsoft.AspNetCore.Mvc;
using SocialNetwok.WebUI.Models;
using System.Diagnostics;

namespace SocialNetwok.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //public IActionResult Notifications()
        //{
        //    return View();
        //}

        //public IActionResult Profile()
        //{
        //    return View();
        //}

        //public IActionResult Messages() 
        //{ 
        //    return View(); 
        //}

        //public IActionResult Friends() 
        //{ 
        //    return View(); 
        //}

        //public IActionResult Events() 
        //{ 
        //    return View(); 
        //}

        //public IActionResult Login() 
        //{ 
        //    return View(); 
        //}

        //public IActionResult Register() 
        //{ 
        //    return View(); 
        //}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
