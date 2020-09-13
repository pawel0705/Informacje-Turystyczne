using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using MailKit.Security;

using InformacjeTurystyczne.Models.InterfaceRepository;
using InformacjeTurystyczne.Models.Tabels;
using InformacjeTurystyczne.Models;
using System.Security.Claims;

namespace InformacjeTurystyczne.Controllers
{
    public class MailController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMessageRepository _messageRepository;

        public MailController(IMessageRepository messageRepository, AppDbContext appDbContext)
        {
            _messageRepository = messageRepository;
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser appUser = await _appDbContext.Users
                .Include(i => i.PermissionRegions)
                .ThenInclude(i => i.Region)
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync(m => m.Id == id);

            PopulateMessage(appUser);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("IdMessage,Name,Description,PostingDate1,IdCategory,IdRegion")] Message message)
        {
            var id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser appUser = await _appDbContext.Users
                .Include(i => i.PermissionRegions)
                .ThenInclude(i => i.Region)
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ModelState.IsValid)
            {
                message.PostingDate1 = DateTime.Now;
                if (appUser.PermissionRegions.Where(p => p.Region.IdRegion == message.IdRegion).Any() || HttpContext.User.IsInRole("Admin"))
                {
                    await _messageRepository.AddMessageAsync(message);
                }

                return RedirectToAction(nameof(Index));
            }

            PopulateMessage(appUser, message.IdRegion, message.IdCategory);

            return View(message);
        }

        public void PopulateMessage(AppUser user = null, object selectedRegion = null, object selectedCategory = null)
        {

            var regionQuery = from r in _messageRepository.GetAllRegionAsNoTracking()
                              orderby r.Name
                              select r;

            var categoryQuery = from c in _messageRepository.GetAllCategoryAsNoTracking()
                                orderby c.Name
                                select c;

            if (HttpContext.User.IsInRole("Admin"))
            {
                ViewBag.IdRegion = new SelectList(regionQuery, "IdRegion", "Name", selectedRegion);
            } 
            else
            {
                /*ViewBag.IdRegion = new SelectList(regionQuery.Intersect(user.PermissionRegions.Select(p => p.Region)),
                    "IdRegion", "Name", selectedRegion);*/
                ViewBag.IdRegion = new SelectList(
                    regionQuery.Where(r => user.PermissionRegions.Select(p => p.Region.Name).Contains(r.Name)),
                    "IdRegion",
                    "Name",
                    selectedRegion
                );
            }
            ViewBag.IdCategory = new SelectList(categoryQuery, "IdCategory", "Name", selectedCategory);
        }
    }
}