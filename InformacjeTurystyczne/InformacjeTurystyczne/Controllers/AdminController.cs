using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using InformacjeTurystyczne.Models.ViewModels;
using InformacjeTurystyczne.Models;
using Microsoft.AspNetCore.Authorization;

namespace InformacjeTurystyczne.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPasswordHasher<AppUser> _passwordHasher;

        public AdminController(UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
        }

        public IActionResult Index()
        {
            ViewData["Users"] = _userManager.Users;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if(user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(string id, string email, string password, bool emailConfirmed)
        {
            var user = await _userManager.FindByIdAsync(id);

            if(user!= null)
            {
                if(!string.IsNullOrEmpty(email))
                {
                    user.Email = email;
                }
                else
                {
                    ModelState.AddModelError("", "Musi być wprowadzony adres email.");
                }

                if(!string.IsNullOrEmpty(password))
                {
                    user.PasswordHash = _passwordHasher.HashPassword(user, password);
                }
                else
                {
                    ModelState.AddModelError("", "Musi być wprowadzone hasło.");
                }

                if(!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    user.EmailConfirmed = emailConfirmed;
                    var result = await _userManager.UpdateAsync(user);

                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ErrorResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("","Użytkownik nie istnieje.");
            }

            return View(user);
        }

        private void ErrorResult(IdentityResult result)
        {
            foreach (var e in result.Errors)
            {
                ModelState.AddModelError("", e.Description);
            }
        }

        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    ErrorResult(result);
            }
            else
                ModelState.AddModelError("", "Taki użytkownik nie istnieje.");
            return View("Index", _userManager.Users);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterVM user)
        {
            if(ModelState.IsValid)
            {
                AppUser appUser = new AppUser
                {
                    UserName = user.UserName,
                    Email = user.Email
                };

                var result = await _userManager.CreateAsync(appUser, user.Password);

                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    foreach(IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }

            return View(user);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return RedirectToAction("AccessDenied", "Account");
        }
    }
}