using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InformacjeTurystyczne.Models.InterfaceRepository;
using InformacjeTurystyczne.Models.Tabels;
using InformacjeTurystyczne.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InformacjeTurystyczne.Controllers.TabelsController
{
    public class RegionController : Controller
    {
        private readonly IRegionRepository _regionRepository;

        public RegionController(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        public async Task<IActionResult> Index(int? id, int? IdParty)
        {
            var regions = _regionRepository.GetAllRegion();

            return View(await regions);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regions = await _regionRepository.GetRegionByID(id);
            if (regions == null)
            {
                return NotFound();
            }

            return View(regions);
        }

        public IActionResult Create()
        {
            var region = new Region();
            region.RegionLocation = new List<RegionLocation>();

            PopulateRegion(region);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRegion,Name")] Region region, string[] selectedTrails)
        {
            if (selectedTrails != null)
            {
                region.RegionLocation = new List<RegionLocation>();
                foreach(var trail in selectedTrails)
                {
                    var trailToAdd = new RegionLocation { IdRegion = region.IdRegion, IdTrail = int.Parse(trail) };
                    region.RegionLocation.Add(trailToAdd);
                }
            }

            if (ModelState.IsValid)
            {
                await _regionRepository.AddRegionAsync(region);

                return RedirectToAction(nameof(Index));
            }

            PopulateRegion(region);

            return View(region);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var region = await _regionRepository.GetRegionByID(id);

            if (region == null)
            {
                return NotFound();
            }

            PopulateRegion(region);

            return View(region);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id, string[] selectedTrails)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regionToUpdate = await _regionRepository.GetRegionByID(id);

            if (await TryUpdateModelAsync<Region>(regionToUpdate,
                    "",
                    c => c.Name))
            {
                UpdateRegion(selectedTrails, regionToUpdate);

                try
                {
                    await _regionRepository.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError(String.Empty, "Nie można zapisać zmian.");
                }

                return RedirectToAction(nameof(Index));
            }

            UpdateRegion(selectedTrails, regionToUpdate);

            return View(regionToUpdate);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var region = await _regionRepository.GetRegionByID(id);

            if (region == null)
            {
                return NotFound();
            }

            return View(region);
        }

        private void PopulateRegion(Region regionToUpdate)
        {
            var allTrails = _regionRepository.GetAllTrails();

            var regionTrail = new HashSet<int?>(regionToUpdate.RegionLocation.Select(c => c.IdTrail));

            var viewModelTrail = new List<PermissionTrailData>();

            foreach(var trail in allTrails)
            {
                viewModelTrail.Add(new PermissionTrailData
                {
                    IdTrail = trail.IdTrail,
                    Name = trail.Name,
                    Assigned = regionTrail.Contains(trail.IdTrail)
                });
            }

            ViewData["Trails"] = viewModelTrail;
        }

        private void UpdateRegion(string[] selectedTrails, Region regionToUpdate)
        {
            if(selectedTrails == null)
            {
                regionToUpdate.RegionLocation = new List<RegionLocation>();

                return;
            }

            var selectedTrailsHS = new HashSet<string>(selectedTrails);
            var regionTrails = new HashSet<int>
                (regionToUpdate.RegionLocation.Select(c => c.Trail.IdTrail));

            foreach(var trail in _regionRepository.GetAllTrails())
            {
                if(selectedTrailsHS.Contains(trail.IdTrail.ToString()))
                {
                    if(!regionTrails.Contains(trail.IdTrail))
                    {
                        regionToUpdate.RegionLocation.Add(new RegionLocation { IdRegion = regionToUpdate.IdRegion, IdTrail = trail.IdTrail });
                    }
                }
                else
                {
                    if(regionTrails.Contains(trail.IdTrail))
                    {
                        RegionLocation trailToRemove = regionToUpdate.RegionLocation.FirstOrDefault(i => i.IdTrail == trail.IdTrail);
                        _regionRepository.RemoveTrail(trailToRemove);
                    }
                }
            }
        }

        // TODO!!! dodać kasowanie odpowiednio zawartości tabeli RegionLocation po usunieciu regionu!

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var region = await _regionRepository.GetRegionByIDWithoutIncludeAndAsNoTracking(id);
            await _regionRepository.DeleteRegionAsync(region);

            return RedirectToAction(nameof(Index));
        }
    }
}