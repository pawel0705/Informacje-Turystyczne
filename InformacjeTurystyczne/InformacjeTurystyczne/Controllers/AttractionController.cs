using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InformacjeTurystyczne.Models.InterfaceRepository;
using InformacjeTurystyczne.Models.Repository;
using InformacjeTurystyczne.Models.Tabels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace InformacjeTurystyczne.Controllers.TabelsController
{
    public class AttractionController : Controller
    {
        private readonly IAttractionRepository _attractionRepository;

        public AttractionController(IAttractionRepository attractionRepository)
        {
            _attractionRepository = attractionRepository;
        }

        public async Task<IActionResult> Index()
        {
            var attractions = _attractionRepository.GetAllAttraction();

            return View(await attractions);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attractions = await _attractionRepository.GetAttractionByID(id);
            if (attractions == null)
            {
                return NotFound();
            }

            return View(attractions);
        }

        public IActionResult Create()
        {
            PopulateAttraction();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAttraction, AttractionType, Name, Description, IdRegion")] Attraction attraction)
        {
            if (ModelState.IsValid)
            {
                await _attractionRepository.AddAttractionAsync(attraction);

                return RedirectToAction(nameof(Index));
            }

            PopulateAttraction(attraction.IdRegion);

            return View(attraction);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attraction = await _attractionRepository.GetAttractionByIDWithoutInclude(id);

            if (attraction == null)
            {
                return NotFound();
            }

            PopulateAttraction(attraction.IdRegion);

            return View(attraction);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attractionToUpdate = await _attractionRepository.GetAttractionByIDWithoutIncludeAndAsNoTracking(id);

            if (await TryUpdateModelAsync<Attraction>(attractionToUpdate,
                    "",
                    c => c.AttractionType, c => c.Name, c => c.Description,  c => c.IdRegion))
            {
                try
                {
                    await _attractionRepository.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError(String.Empty, "Nie można zapisać zmian.");
                }

                return RedirectToAction(nameof(Index));
            }

            PopulateAttraction(attractionToUpdate.IdRegion);
            return View(attractionToUpdate);
        }

        public void PopulateAttraction(object selectedRegion = null)
        {

            var regionQuery = from e in _attractionRepository.GetAllRegionAsNoTracking()
                              orderby e.Name
                              select e;

            ViewBag.IdRegion = new SelectList(regionQuery, "IdRegion", "Name", selectedRegion);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attraction = await _attractionRepository.GetAttractionByID(id);

            if (attraction == null)
            {
                return NotFound();
            }

            return View(attraction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _attractionRepository.GetAttractionByIDWithoutIncludeAndAsNoTracking(id);
            await _attractionRepository.DeleteAttractionAsync(course);

            return RedirectToAction(nameof(Index));
        }
    }
}