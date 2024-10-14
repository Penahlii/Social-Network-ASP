using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwok.Entities.Entities;
using SocialNetwork.Business.Abstract;
using SocialNetwork.WebUI.Models;
using System.Diagnostics;

namespace SocialNetwork.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
		private readonly UserManager<CustomIdentityUser> _userManager;
		public HomeController(UserManager<CustomIdentityUser> userManager)
		{
			_userManager = userManager;
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

		public IActionResult Notifications() => View();

		public IActionResult Friends() => View();

		public IActionResult Messages() => View();

		public async Task<IActionResult> Index()
		{
			var currentUser = await _userManager.GetUserAsync(HttpContext.User);
			if (currentUser != null) return View();
			return RedirectToAction("Login", "Account");
		}
	}
}
