using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InformacjeTurystyczne.Models.InterfaceRepository;
using InformacjeTurystyczne.Models.Tabels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InformacjeTurystyczne.Controllers.TabelsController
{
    [System.Runtime.InteropServices.Guid("28B2BD28-2FAF-47D2-A4DD-AB28DADED1F0")]
    public class ShelterController : Controller
    {
        private readonly IShelterRepository _shelterRepository;

        public ShelterController(IShelterRepository shelterRepository)
        {
            _shelterRepository = shelterRepository;
        }

        public async Task<IActionResult> Index()
        {
            var shelters = _shelterRepository.GetAllShelter();

            return View(await shelters);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelters = await _shelterRepository.GetShelterByID(id);
            if (shelters == null)
            {
                return NotFound();
            }

            return View(shelters);
        }

        public IActionResult Create()
        {
            PopulateShelter();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdShelter, Name, MaxPlaces, Places, IsOpen, PhoneNumber, Description, IdRegion")] Shelter shelter)
        {
            if (ModelState.IsValid)
            {
                await _shelterRepository.AddShelterAsync(shelter);

                return RedirectToAction(nameof(Index));
            }

            PopulateShelter(shelter.IdRegion);

            return View(shelter);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelter = await _shelterRepository.GetShelterByIDWithoutInclude(id);

            if (shelter == null)
            {
                return NotFound();
            }

            PopulateShelter(shelter.IdRegion);

            return View(shelter);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelterToUpdate = await _shelterRepository.GetShelterByIDWithoutIncludeAndAsNoTracking(id);

            if (await TryUpdateModelAsync<Shelter>(shelterToUpdate,
                    "",
                    c => c.Name, c => c.MaxPlaces, c => c.Places, c => c.IsOpen, c=>c.Description, c => c.IdRegion))
            {
                try
                {
                    await _shelterRepository.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError(String.Empty, "Nie można zapisać zmian.");
                }

                return RedirectToAction(nameof(Index));
            }

            PopulateShelter(shelterToUpdate.IdRegion);
            return View(shelterToUpdate);
        }

        public void PopulateShelter(object selectedRegion = null)
        {

            var regionQuery = from e in _shelterRepository.GetAllRegionAsNoTracking()
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

            var shelter = await _shelterRepository.GetShelterByID(id);

            if (shelter == null)
            {
                return NotFound();
            }

            return View(shelter);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shelter = await _shelterRepository.GetShelterByIDWithoutIncludeAndAsNoTracking(id);
            await _shelterRepository.DeleteShelterAsync(shelter);

            return RedirectToAction(nameof(Index));
        }
    }
}
