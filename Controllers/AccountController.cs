using EmployeeMangements.Models;
using EmployeeMangements.viewModles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EmployeeMangements.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signlnManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signlnManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            this.userManager = userManager;
            this.signlnManager = signlnManager;
            this.roleManager = roleManager;
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signlnManager.SignOutAsync();
             return RedirectToAction("index","home");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AcceptVerbs("Get","Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use");
            }
        }


        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    //UserName = new MailAddress(model.Email).User,
                    UserName = model.Email,
                    Email = model.Email,
                    City = model.City,
                };
                // CreateAsync use to create new user 
               var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (signlnManager.IsSignedIn(User) && User.IsInRole("admin"))
                    {
                        await userManager.AddToRoleAsync(user, "user");
                        return RedirectToAction("GetUsers", "Adminstration");
                       
                    }
                    await userManager.AddToRoleAsync(user, "admin");
                    await signlnManager.SignInAsync(user,false);
                    return RedirectToAction("index", "home");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            
            return View();
        }
        [HttpPost]  
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                var result = await signlnManager.PasswordSignInAsync(model.Email,model.Password,
                    model.RememberMe,false);
                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }
               
                    ModelState.AddModelError("", "Invaild Login Attempt");
                
            }
            return View();
        }
        //[HttpPost]
        //[AllowAnonymous]
        //public IActionResult ExternalLogin(string provider , string returnUrl)
        //{
        //    var redirectUrl = Url.Action("ExternalLoginCallback", "account",
        //        new { ReturnUrl = returnUrl });
        //    var properties = signlnManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        //    return new ChallengeResult(provider, properties);
        //}
        //[AllowAnonymous]
        //public async Task<IActionResult>
        //    ExternalLoginCallback(string returnUrl =null ,string remoteError = null)
        //{
        //    returnUrl = returnUrl ?? Url.Content("~/");
        //    LoginViewModel model = new LoginViewModel
        //    {
        //        ReturnUrl = returnUrl,
        //        ExternalLogins = (await signlnManager.GetExternalAuthenticationSchemesAsync()).ToList()
        //    };
        //    if (remoteError != null)
        //    {
        //        ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
        //        return View("Login", model);
        //    }
        //    var info = await signlnManager.GetExternalLoginInfoAsync();
        //    if (info == null)
        //    {
        //        ModelState.AddModelError(string.Empty, $"Error loading external login information.");
        //        return View("Login", model); 
        //    }
        //    var signInResult = await signlnManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey
        //        , isPersistent: false, bypassTwoFactor: true);
        //    if (signInResult.Succeeded)
        //    {
        //        return LocalRedirect(returnUrl);
        //    }
        //    return View("Login", model);
        //}


    }
}
