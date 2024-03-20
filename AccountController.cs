using LaptopStoreOnline.Models;
using LaptopStoreOnline.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LaptopStoreOnline.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            this._signInManager = signInManager;
            _userManager = userManager;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                //login
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RemeberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid login attempt");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser()
                {
                    Name = model.Name,
                    UserName = model.Email,
                    Email = model.Email,
                    Address = model.Address
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                //ModelState.AddModelError("", result.Errors.ToString());
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
                }
                return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager?.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
