using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using InformacjeTurystyczne.Models;
using InformacjeTurystyczne.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InformacjeTurystyczne.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<ViewResult> Index()
        {
            var adminRole = await  _roleManager.FindByNameAsync("Admin");

            if(adminRole == null)
            {
                var identityRole = new IdentityRole
                {
                    Name = "Admin"
                };

                await _roleManager.CreateAsync(identityRole);
            }

            var moderatorRole = await _roleManager.FindByNameAsync("Moderator");

            if (moderatorRole == null)
            {
                var identityRole = new IdentityRole
                {
                    Name = "Moderator"
                };

                await _roleManager.CreateAsync(identityRole);
            }

            return View(_roleManager.Roles);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleVM roleVM)
        {
            if(ModelState.IsValid)
            {
                var identityRole = new IdentityRole
                {
                    Name = roleVM.RoleName
                };

                var result = await _roleManager.CreateAsync(identityRole);

                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ResultErrors(result);
                }
            }

            return View(roleVM);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(RoleVM roleVM)
        {
            var role = await _roleManager.FindByIdAsync(roleVM.Id);

            if(role!= null)
            {
                var result = await _roleManager.DeleteAsync(role);

                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ResultErrors(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "Nie znaleziono roli.");
            }

            return View("Index", _roleManager.Roles);
        }

        public async Task<IActionResult> UpdateRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            List<AppUser> membersList = new List<AppUser>();
            List<AppUser> nonMembersList = new List<AppUser>();

            foreach (var user in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? membersList : nonMembersList;

                list.Add(user);
            }

            return View(new RoleUsersVM { Role = role, Members = membersList, NonMembers = nonMembersList });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(RoleVM roleVM)
        {
            if(ModelState.IsValid)
            {
                foreach (var id in roleVM.AddId ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(id);

                    if(user != null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, roleVM.RoleName);

                        if(!result.Succeeded)
                        {
                            ResultErrors(result);
                        }
                    }
                }

                foreach (var id in roleVM.DeleteId ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(id);

                    if(user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, roleVM.RoleName);

                        if(!result.Succeeded)
                        {
                            ResultErrors(result);
                        }
                    }
                }
            }

            if(ModelState.IsValid)
            {
                return RedirectToAction("Index", "Role");
            }
            else
            {
                return await UpdateRole(roleVM.Id);
            }
        }

        private void ResultErrors(IdentityResult result)
        {
            foreach (var e in result.Errors)
            {
                ModelState.AddModelError("", e.Description);
            }
        }
    }
}