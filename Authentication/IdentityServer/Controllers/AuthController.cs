using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel {
                returnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            var result = await _signInManager.PasswordSignInAsync(loginViewModel.username, loginViewModel.password, false, false);

            if(result.Succeeded)
            {
                return Redirect(loginViewModel.returnUrl);
            }
            else if(result.IsLockedOut)
            {

            }

            return View(loginViewModel);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel
            {
                returnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            var user = new IdentityUser(registerViewModel.username);

            var result = await _userManager.CreateAsync(user, registerViewModel.password);


            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Redirect(registerViewModel.returnUrl);
            }
            return View(registerViewModel);

        }
    }
}
