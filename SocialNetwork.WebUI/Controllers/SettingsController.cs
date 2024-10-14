using Microsoft.AspNetCore.Mvc;

namespace SocialNetwork.WebUI.Controllers;

public class SettingsController : Controller
{
	public IActionResult MyProfile()
	{
		return View();
	}

	public IActionResult Setting()
	{
		return View();
	}

	public IActionResult Privacy()
	{
		return View();
	}

	public IActionResult HelpAndSupport()
	{
		return View();
	}
}
