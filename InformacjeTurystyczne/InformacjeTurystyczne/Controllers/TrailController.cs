using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InformacjeTurystyczne.Models;
using InformacjeTurystyczne.Models.InterfaceRepository;
using InformacjeTurystyczne.Models.Tabels;
using InformacjeTurystyczne.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InformacjeTurystyczne.Controllers.TabelsController
{
    public class TrailController : Controller
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly AppDbContext _appDbContext;

        public TrailController(ITrailRepository trailRepository, IRegionRepository regionRepository, AppDbContext appDbContext)
        {
            _trailRepository = trailRepository;
            _regionRepository = regionRepository;
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var trails = _trailRepository.GetAllTrail();

            return View(await trails);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trails = await _trailRepository.GetTrailByID(id);
            if (trails == null)
            {
                return NotFound();
            }

            return View(trails);
        }

        public IActionResult Create()
        {
            List<RegionSelection> regions = new List<RegionSelection>();
            foreach (var region in _regionRepository.GetAllRegionToUser())
            {
                regions.Add(new RegionSelection
                {
                    Name = region.Name,
                    Selected = false
                });
            }
            ViewBag.Regions = regions;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTrail, Name, Colour, Open, Feedback, Length, Difficulty, Description, RegionLocation")] Trail trail, string[] selectedRegions)
        {
            if (ModelState.IsValid)
            {
                await _trailRepository.AddTrailAsync(trail);

                trail.RegionLocation = new List<RegionLocation>();
                foreach (var region in selectedRegions)
                {
                    trail.RegionLocation.Add(new RegionLocation
                    {
                        IdRegion = _regionRepository.GetAllRegionToUser().Where(r => r.Name == region).FirstOrDefault().IdRegion,
                        IdTrail = trail.IdTrail
                    });
                }
                _trailRepository.EditTrail(trail);

                return RedirectToAction(nameof(Index));
            }
            List<RegionSelection> regions = new List<RegionSelection>();
            foreach (var region in _regionRepository.GetAllRegionToUser())
            {
                regions.Add(new RegionSelection
                {
                    Name = region.Name,
                    Selected = selectedRegions.Contains(region.Name)
                });
            }
            ViewBag.Regions = regions;


            return View(trail);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var trail = await _trailRepository.GetTrailByIDWithoutInclude(id);
            var trail = await _appDbContext.Trails
                .Include(t => t.RegionLocation)
                .FirstOrDefaultAsync(s => s.IdTrail == id);

            if (trail == null)
            {
                return NotFound();
            }

            List<RegionSelection> regions = new List<RegionSelection>();
            foreach (var region in _regionRepository.GetAllRegionToUser())
            {
                regions.Add(new RegionSelection
                {
                    Name = region.Name,
                    Selected = trail.RegionLocation.Select(rl => rl.IdRegion).Contains(region.IdRegion)
                });
            }
            ViewBag.Regions = regions;

            return View(trail);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id, string[] selectedRegions)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var trailToUpdate = await _trailRepository.GetTrailByIDWithoutIncludeAndAsNoTracking(id);

            var trailToUpdate = await _appDbContext.Trails
                .Include(t => t.RegionLocation)
                .FirstOrDefaultAsync(s => s.IdTrail == id);

            if (await TryUpdateModelAsync<Trail>(trailToUpdate,
                    "",
                    c=>c.Name, c => c.Colour, c=>c.Open, c=>c.Feedback, c=>c.Length, c=>c.Difficulty, c=>c.Description))
            {
                try
                {
                    trailToUpdate.RegionLocation = new List<RegionLocation>();
                    foreach (var region in selectedRegions)
                    {
                        trailToUpdate.RegionLocation.Add(new RegionLocation
                        {
                            IdRegion = _regionRepository.GetAllRegionToUser().Where(r => r.Name == region).FirstOrDefault().IdRegion,
                            IdTrail = trailToUpdate.IdTrail
                        });
                    }
                    _trailRepository.EditTrail(trailToUpdate);
                    await _trailRepository.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError(String.Empty, "Nie można zapisać zmian.");
                }

                return RedirectToAction(nameof(Index));
            }

            List<RegionSelection> regions = new List<RegionSelection>();
            foreach (var region in _regionRepository.GetAllRegionToUser())
            {
                regions.Add(new RegionSelection
                {
                    Name = region.Name,
                    Selected = trailToUpdate.RegionLocation.Select(rl => rl.IdRegion).Contains(region.IdRegion)
                });
            }

            ViewBag.Regions = regions;

            return View(trailToUpdate);
        }

       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trail = await _trailRepository.GetTrailByID(id);

            if (trail == null)
            {
                return NotFound();
            }

            return View(trail);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trail = await _trailRepository.GetTrailByIDWithoutIncludeAndAsNoTracking(id);
            await _trailRepository.DeleteTrailAsync(trail);

            return RedirectToAction(nameof(Index));
        }
    }
}