using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InformacjeTurystyczne.Models.InterfaceRepository;
using InformacjeTurystyczne.Models.ViewModels;
using InformacjeTurystyczne.Models;
using System.Security.Claims;
using InformacjeTurystyczne.Models.Tabels;
using Microsoft.AspNetCore.Identity;

namespace InformacjeTurystyczne.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly AppDbContext _appDbContext;

        public NotificationsController(IMessageRepository messageRepository, IRegionRepository regionRepository, AppDbContext appDbContext)
        {
            _messageRepository = messageRepository;
            _regionRepository = regionRepository;
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var viewModel = new NotificationsVM();
            if (HttpContext.User != null && HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                var id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                AppUser appUser = await _appDbContext.Users
                    .Include(i => i.Subscriptions)
                    .ThenInclude(i => i.Region)
                    .Include(i => i.Subscriptions)
                    .ThenInclude(i => i.User)
                    .FirstOrDefaultAsync(m => m.Id == id);
                viewModel.user = appUser;
            }
            else
            {
                viewModel.user = null;
            }
            viewModel.messages = _messageRepository;
            viewModel.regions = _regionRepository.GetAllRegionToUser().ToList();

            return View(viewModel);
        }
        public class RegionData
        {
            public string RegionName { get; set; }
        }

        [HttpPost]
        public async Task<EmptyResult> AddSubscription([FromBody] RegionData data)
        {
            if (HttpContext.User == null)
                return new EmptyResult();
            var id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser userToUpdate = await _appDbContext.Users
                .Include(i => i.Subscriptions)
                .ThenInclude(i => i.Region)
                .Include(i => i.Subscriptions)
                .ThenInclude(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            Region region = _regionRepository.GetAllRegionToUser().Where(r => r.Name == data.RegionName).Single();

            if (!userToUpdate.Subscriptions.Select(p => p.Region.Name).Contains(data.RegionName))
            {
                userToUpdate.Subscriptions.Add(new Subscription
                {
                    IdUser = userToUpdate.Id,
                    IdRegion = region.IdRegion,
                    IsSubscribed = true
                });
            }

            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {

            }
            return new EmptyResult();
        }
        [HttpPost]
        public async Task<EmptyResult> RemoveSubscription([FromBody] RegionData data)
        {
            if (HttpContext.User == null)
                return new EmptyResult();
            var id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser userToUpdate = await _appDbContext.Users
                .Include(i => i.Subscriptions)
                .ThenInclude(i => i.Region)
                .Include(i => i.Subscriptions)
                .ThenInclude(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userToUpdate.Subscriptions.Select(p => p.Region.Name).Contains(data.RegionName))
            {
                userToUpdate.Subscriptions.Remove(
                    userToUpdate.Subscriptions.Where(s => s.Region.Name == data.RegionName).Single()
                );
            }

            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {

            }

            return new EmptyResult();
        }
    }
}