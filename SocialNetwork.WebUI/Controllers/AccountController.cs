using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwok.Entities.Entities;
using SocialNetwork.Business.Abstract;
using SocialNetwork.WebUI.Models;
using SocialNetwork.WebUI.Services.Abstract;

namespace SocialNetwork.WebUI.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<CustomIdentityUser> _userManager;
    private readonly RoleManager<CustomIdentityRole> _roleManager;
    private readonly SignInManager<CustomIdentityUser> _signInManager;
    private readonly IUserService _userService;
    private readonly IPhotoService _photoService;

    public AccountController(UserManager<CustomIdentityUser> userManager, RoleManager<CustomIdentityRole> roleManager, SignInManager<CustomIdentityUser> signInManager, IUserService userService, IPhotoService photoService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _userService = userService;
        _photoService = photoService;
    }

    public ActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            string imagePath = "";
            if (model.ProfileImage != null)
            {
                imagePath = await _photoService.UploadImageAsync(model.ProfileImage);
            }

            CustomIdentityUser user = new CustomIdentityUser
            {
                UserName = model.Username,
                Email = model.Email,
                ImageUrl = imagePath,
                City = model.City
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Client"))
                {
                    CustomIdentityRole role = new CustomIdentityRole
                    {
                        Name = "Client"
                    };

                    IdentityResult roleResult = await _roleManager.CreateAsync(role);
                    if (!roleResult.Succeeded)
                    {
                        ModelState.AddModelError("", "Failed to Add the new Role");
                    }
                }
            }
            await _userManager.AddToRoleAsync(user, "Client");
            return RedirectToAction("Login", "Account");
        }
        return View(model);
    }

    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                var user = await _userService.GetUserAsync(u => u.UserName == model.Username);
                user.IsOnline = true;
                await _userService.UpdateUserAsync(user);
                ViewBag.ImagePath = user.Image;
            }
            return RedirectToAction("Index", "Home");
        }
        ModelState.AddModelError("", "Invalid Login");
        return View(model);
    }

    public async Task<IActionResult> LogOut()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        user.DisconnectTime = DateTime.Now;
        user.IsOnline = false;
        await _userService.UpdateUserAsync(user);
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
    {
        var currentUser = await _userManager.GetUserAsync(HttpContext.User);
        var isPasswordCorrect = await _userManager.CheckPasswordAsync(currentUser, model.OldPassword);

        if (isPasswordCorrect)
        {
            var result = await _userManager.ChangePasswordAsync(currentUser, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return BadRequest("Failed to change password.");
            }
        }
        return BadRequest("Invalid password.");
    }

    [HttpPost]
    public async Task<IActionResult> EditUser([FromBody] EditUserViewModel model)
    {
        var currentUser = await _userManager.GetUserAsync(HttpContext.User);
        if (model.Email.EndsWith("@gmail.com") && model.Email.Length > 5 && model.Username.Length > 4)
        {
            currentUser.UserName = model.Username;
            currentUser.Email = model.Email;
            await _userManager.UpdateAsync(currentUser);
            return RedirectToAction("Index", "Home");
        }
        return BadRequest("can not edit the user cause of invalid username and email !");
    }

    [HttpGet]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null) return BadRequest();
        var obj = new { User = user };
        return Ok(obj);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers(string? key)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null) return BadRequest();
        dynamic? allUsersExpectCurrent;
        allUsersExpectCurrent = string.IsNullOrEmpty(key) ? await _userManager.Users.Where(u => u.Id != user.Id).OrderByDescending(u => u.IsOnline).ToListAsync() : await _userManager.Users.Where(u => u.Id != user.Id && u.UserName.Contains(key)).OrderByDescending(u => u.IsOnline).ToListAsync();
        return Ok(new { AllUsers = allUsersExpectCurrent });
    }
        
    [HttpGet]
    public async Task<IActionResult> GetUserByUsername(string username)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
        if (user == null) return BadRequest();
        return Ok(new { User = user });
    }

    [HttpGet]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return BadRequest();
        return Ok(new { User = user });
    }
}
