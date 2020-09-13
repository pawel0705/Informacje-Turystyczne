using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using InformacjeTurystyczne.Models;
using InformacjeTurystyczne.Models.InterfaceRepository;
using InformacjeTurystyczne.Models.Tabels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InformacjeTurystyczne.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IRegionRepository _regionRepository;

        public UserController(AppDbContext appDbContext, IRegionRepository regionRepository)
        {
            _appDbContext = appDbContext;
            _regionRepository = regionRepository;
        }
        public async Task<IActionResult> Index()
        {
            if (HttpContext.User == null)
            {
                return RedirectToAction("Home", "Index");
            }
            var id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser appUser = await _appDbContext.Users
                .Include(i => i.PermissionRegions)
                .ThenInclude(i => i.Region)
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync(m => m.Id == id);

            dynamic viewModel = new ExpandoObject();
            viewModel.users = await _appDbContext.Users
                .Include(u => u.PermissionRegions)
                .ThenInclude(i => i.Region)
                .ToListAsync();
            if (!User.IsInRole("Admin"))
            {
                foreach (var user in (List<AppUser>)viewModel.users)
                {
                    user.PermissionRegions = user.PermissionRegions
                        .Where(r => appUser.PermissionRegions.Select(p => p.Region.Name).Contains(r.Region.Name)).ToList();
                }
                viewModel.permissions = appUser.PermissionRegions.Select(p => p.Region).ToList();
            }
            else
            {
                viewModel.permissions = _regionRepository.GetAllRegionToUser().ToList();
            }
            /*viewModel.users = await _appDbContext.Users
                .Include(u => u.PermissionRegions.Where(r => appUser.PermissionRegions.Select(p => p.Region.Name).Contains(r.Region.Name)))
                .ThenInclude(i => i.Region)
                .ToListAsync();*/
            return View(viewModel);
        }

        public class RegionData
        {
            public string Id { get; set; }
            public string RegionName { get; set; }
        }

        [HttpPost]
        public async Task<EmptyResult> AddRegionPermission([FromBody] RegionData data)
        {
            AppUser userToUpdate = await _appDbContext.Users
                .Include(i => i.PermissionRegions)
                .ThenInclude(i => i.Region)
                .Where(i => i.Id == data.Id)
                .FirstOrDefaultAsync(m => m.Id == data.Id);
            if (!userToUpdate.PermissionRegions.Select(p => p.Region.Name).Contains(data.RegionName))
            {
                var IdUser = userToUpdate.Id;
                var IdRegion = _appDbContext.Regions.Where(r => r.Name == data.RegionName).Single().IdRegion;
                userToUpdate.PermissionRegions.Add(new PermissionRegion
                {
                    IdUser = userToUpdate.Id,
                    IdRegion = _appDbContext.Regions.Where(r => r.Name == data.RegionName).Single().IdRegion
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
        public async Task<EmptyResult> RemoveRegionPermission([FromBody] RegionData data)
        {
            AppUser userToUpdate = await _appDbContext.Users
                .Include(i => i.PermissionRegions)
                .ThenInclude(i => i.Region)
                .Where(i => i.Id == data.Id)
                .FirstOrDefaultAsync(m => m.Id == data.Id);
            if (userToUpdate.PermissionRegions.Select(p => p.Region.Name).Contains(data.RegionName))
            {
                userToUpdate.PermissionRegions.Remove(
                    userToUpdate.PermissionRegions.Where(p => p.Region.Name == data.RegionName).Single()
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