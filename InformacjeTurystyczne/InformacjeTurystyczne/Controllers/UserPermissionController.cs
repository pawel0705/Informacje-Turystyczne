using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InformacjeTurystyczne.Models;
using InformacjeTurystyczne.Models.Tabels;
using InformacjeTurystyczne.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// TODO!!! dodać kasowanie odpowiednio tabel Permission[...] po kasowaniu użytkownika, by nie zostawały z martwym ID w bazie danych!

namespace InformacjeTurystyczne.Controllers
{
    public class UserPermissionController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public UserPermissionController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index(string id, int? IdRegion, int? IdTrail, int? IdShelter, int? IdParty)
        {
            var viewModel = new UserIndexData();
            viewModel.Users = await _appDbContext.Users
                .Include(i => i.PermissionRegions)
                .ThenInclude(i => i.Region)
                .Include(i => i.PermissionTrails)
                .ThenInclude(i => i.Trail)
                .Include(i => i.PermissionShelters)
                .ThenInclude(i => i.Shelter)
                .Include(i => i.PermissionPartys)
                .ThenInclude(i => i.Party)
                .OrderBy(i => i.UserName)
                .ToListAsync();

            if (id != null)
            {
                ViewData["IdUser"] = id;

                AppUser user = viewModel.Users.Where(i => i.Id == id).Single();

                viewModel.Partys = user.PermissionPartys.Select(s => s.Party);
                viewModel.Regions = user.PermissionRegions.Select(s => s.Region);
                viewModel.Shelters = user.PermissionShelters.Select(s => s.Shelter);
                viewModel.Trails = user.PermissionTrails.Select(s => s.Trail);
            }

            /*
            foreach(var user in _appDbContext.Users)
            {
                if(user.PermissionPartys == null)
                {
                    user.PermissionPartys = new List<PermissionParty>();
                    PopulateUser(user);
                }
            }
            await _appDbContext.SaveChangesAsync();
            */
            /*
            if(IdRegion != null)
            {
                ViewData["IdRegion"] = IdRegion.Value;
                var selectedRegion = viewModel.Regions.Where(x => x.IdRegion == IdRegion).Single();

            }
            */

            return View(viewModel);
        }

        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _appDbContext.Users
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult Create()
        {
            var user = new AppUser();
            user.PermissionPartys = new List<PermissionParty>();
            user.PermissionRegions = new List<PermissionRegion>();
            user.PermissionShelters = new List<PermissionShelter>();
            user.PermissionTrails = new List<PermissionTrail>();

            PopulateUser(user);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppUser user, string[] selectedPartys, string[] selectedRegions, string[] selectedShelters, string[] selectedTrails)
        {
            if (selectedPartys != null)
            {
                user.PermissionPartys = new List<PermissionParty>();
                foreach (var party in selectedPartys)
                {
                    var partyToAdd = new PermissionParty { IdUser = user.Id, IdParty = int.Parse(party) };
                    user.PermissionPartys.Add(partyToAdd);
                }
            }

            if (selectedRegions != null)
            {
                user.PermissionRegions = new List<PermissionRegion>();
                foreach (var region in selectedRegions)
                {
                    var regionToAdd = new PermissionRegion { IdUser = user.Id, IdRegion = int.Parse(region) };
                    user.PermissionRegions.Add(regionToAdd);
                }
            }

            if (selectedShelters != null)
            {
                user.PermissionShelters = new List<PermissionShelter>();
                foreach (var shelter in selectedShelters)
                {
                    var shelterToAdd = new PermissionShelter { IdUser = user.Id, IdShelter = int.Parse(shelter) };
                    user.PermissionShelters.Add(shelterToAdd);
                }
            }

            if (selectedTrails != null)
            {
                user.PermissionTrails = new List<PermissionTrail>();
                foreach (var trail in selectedTrails)
                {
                    var trailToAdd = new PermissionTrail { IdUser = user.Id, IdTrail = int.Parse(trail) };
                    user.PermissionTrails.Add(trailToAdd);
                }
            }

            if (ModelState.IsValid)
            {
                _appDbContext.Add(user);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateUser(user);

            return View(user);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _appDbContext.Users
                .Include(i => i.PermissionRegions)
                .ThenInclude(i => i.Region)
                .Include(i => i.PermissionTrails)
                .ThenInclude(i => i.Trail)
                .Include(i => i.PermissionShelters)
                .ThenInclude(i => i.Shelter)
                .Include(i => i.PermissionPartys)
                .ThenInclude(i => i.Party)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            PopulateUser(user);

            return View(user);
        }

        private void PopulateUser(AppUser userToUpdate)
        {
            var allPartys = _appDbContext.Partys;
            var allRegions = _appDbContext.Regions;
            var allShelters = _appDbContext.Shelters;
            var allTrails = _appDbContext.Trails;

            var userParty = new HashSet<int>(userToUpdate.PermissionPartys.Select(c => c.IdParty));
            var userRegion = new HashSet<int?>(userToUpdate.PermissionRegions.Select(c => c.IdRegion));
            var userShelter = new HashSet<int?>(userToUpdate.PermissionShelters.Select(c => c.IdShelter));
            var userTrail = new HashSet<int?>(userToUpdate.PermissionTrails.Select(c => c.IdTrail));

            var viewModelParty = new List<PermissionPartyData>();
            var viewModelRegion = new List<PermissionRegionData>();
            var viewModelShelter = new List<PermissionShelterData>();
            var viewModelTrail = new List<PermissionTrailData>();

            foreach (var party in allPartys)
            {
                viewModelParty.Add(new PermissionPartyData
                {
                    IdParty = party.IdParty,
                    Name = party.Name,
                    Assigned = userParty.Contains(party.IdParty)
                });
            }

            ViewData["Partys"] = viewModelParty;

            foreach (var region in allRegions)
            {
                viewModelRegion.Add(new PermissionRegionData
                {
                    IdRegion = region.IdRegion,
                    Name = region.Name,
                    Assigned = userRegion.Contains(region.IdRegion)
                });
            }

            ViewData["Regions"] = viewModelRegion;

            foreach (var shelter in allShelters)
            {
                viewModelShelter.Add(new PermissionShelterData
                {
                    IdShelter = shelter.IdShelter,
                    Name = shelter.Name,
                    Assigned = userShelter.Contains(shelter.IdShelter)
                });
            }

            ViewData["Shelters"] = viewModelShelter;

            foreach (var trail in allTrails)
            {
                viewModelTrail.Add(new PermissionTrailData
                {
                    IdTrail = trail.IdTrail,
                    Name = trail.Name,
                    Assigned = userShelter.Contains(trail.IdTrail)
                });
            }

            ViewData["Trails"] = viewModelTrail;
        }

        private void UpdateUser(string[] selectedPartys, string[] selectedRegions,
            string[] selectedShelters, string[] selectedTrails, AppUser userToUpdate)
        {
            if (selectedPartys == null || selectedRegions == null ||
                selectedShelters == null || selectedTrails == null)
            {
                if(selectedPartys == null)
                {
                    userToUpdate.PermissionPartys = new List<PermissionParty>();
                }
                if(selectedRegions == null)
                {
                    userToUpdate.PermissionRegions = new List<PermissionRegion>();
                }
                if(selectedShelters == null)
                {
                    userToUpdate.PermissionShelters = new List<PermissionShelter>();
                }
                if(selectedTrails == null)
                {
                    userToUpdate.PermissionTrails = new List<PermissionTrail>();
                }
                
                return;
            }

            var selectedPartysHS = new HashSet<string>(selectedPartys);
            var userPartys = new HashSet<int>
                (userToUpdate.PermissionPartys.Select(c => c.Party.IdParty));

            foreach (var party in _appDbContext.Partys)
            {
                if (selectedPartysHS.Contains(party.IdParty.ToString()))
                {
                    if (!userPartys.Contains(party.IdParty))
                    {
                        userToUpdate.PermissionPartys.Add(new PermissionParty { IdUser = userToUpdate.Id, IdParty = party.IdParty });

                    }
                }
                else
                {
                    if (userPartys.Contains(party.IdParty))
                    {
                        PermissionParty partyToRemove = userToUpdate.PermissionPartys.FirstOrDefault(i => i.IdParty == party.IdParty);
                        _appDbContext.Remove(partyToRemove);
                    }
                }
            }

            var selectedRegionsHS = new HashSet<string>(selectedRegions);
            var userRegions = new HashSet<int>
                (userToUpdate.PermissionRegions.Select(c => c.Region.IdRegion));

            foreach (var region in _appDbContext.Regions)
            {
                if (selectedRegionsHS.Contains(region.IdRegion.ToString()))
                {
                    if (!userRegions.Contains(region.IdRegion))
                    {
                        userToUpdate.PermissionRegions.Add(new PermissionRegion { IdUser = userToUpdate.Id, IdRegion = region.IdRegion });

                    }
                }
                else
                {
                    if (userRegions.Contains(region.IdRegion))
                    {
                        PermissionRegion regionToRemove = userToUpdate.PermissionRegions.FirstOrDefault(i => i.IdRegion == region.IdRegion);
                        _appDbContext.Remove(regionToRemove);
                    }
                }
            }

            var selectedSheltersHS = new HashSet<string>(selectedShelters);
            var userShelters = new HashSet<int>
                (userToUpdate.PermissionShelters.Select(c => c.Shelter.IdShelter));

            foreach (var shelter in _appDbContext.Shelters)
            {
                if (selectedSheltersHS.Contains(shelter.IdShelter.ToString()))
                {
                    if (!userShelters.Contains(shelter.IdShelter))
                    {
                        userToUpdate.PermissionShelters.Add(new PermissionShelter { IdUser = userToUpdate.Id, IdShelter = shelter.IdShelter });

                    }
                }
                else
                {
                    if (userShelters.Contains(shelter.IdShelter))
                    {
                        PermissionShelter shelterToRemove = userToUpdate.PermissionShelters.FirstOrDefault(i => i.IdShelter == shelter.IdShelter);
                        _appDbContext.Remove(shelterToRemove);
                    }
                }
            }

            var selectedTrailsHS = new HashSet<string>(selectedTrails);
            var userTrails = new HashSet<int>
                (userToUpdate.PermissionTrails.Select(c => c.Trail.IdTrail));

            foreach (var trail in _appDbContext.Trails)
            {
                if (selectedTrailsHS.Contains(trail.IdTrail.ToString()))
                {
                    if (!userTrails.Contains(trail.IdTrail))
                    {
                        userToUpdate.PermissionTrails.Add(new PermissionTrail { IdUser = userToUpdate.Id, IdTrail = trail.IdTrail });

                    }
                }
                else
                {
                    if (userTrails.Contains(trail.IdTrail))
                    {
                        PermissionTrail trailToRemove = userToUpdate.PermissionTrails.FirstOrDefault(i => i.IdTrail == trail.IdTrail);
                        _appDbContext.Remove(trailToRemove);
                    }
                }
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string[] selectedPartys, string[] selectedRegions, 
            string[] selectedShelters, string[] selectedTrails)
        {
            if (id == null)
            {
                return NotFound();
            }
       
            var userToUpdate = await _appDbContext.Users
                .Include(i => i.PermissionRegions)
                .ThenInclude(i => i.Region)
                .Include(i => i.PermissionTrails)
                .ThenInclude(i => i.Trail)
                .Include(i => i.PermissionShelters)
                .ThenInclude(i => i.Shelter)
                .Include(i => i.PermissionPartys)
                .ThenInclude(i => i.Party)
                .FirstOrDefaultAsync(m => m.Id == id);


            if (userToUpdate != null)
            {
                UpdateUser(selectedPartys, selectedRegions, 
                    selectedShelters, selectedTrails, userToUpdate);

                try
                {
                    await _appDbContext.SaveChangesAsync();
                }
                catch(DbUpdateException ex)
                {
                    ModelState.AddModelError(String.Empty, "Nie udało się zaktualizować.");
                }

                return RedirectToAction(nameof(Index));
            }

            UpdateUser(selectedPartys, selectedRegions,
                    selectedShelters, selectedTrails, userToUpdate);

            return View(userToUpdate);
        }
    }
}