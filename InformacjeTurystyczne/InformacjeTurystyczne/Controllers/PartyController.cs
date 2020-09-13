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
    public class PartyController : Controller
    {
        private readonly IPartyRepository _partyRepository;

        public PartyController(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }

        public async Task<IActionResult> Index()
        {
            var partys = _partyRepository.GetAllParty();

            return View(await partys);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partys = await _partyRepository.GetPartyByID(id);
            if (partys == null)
            {
                return NotFound();
            }

            return View(partys);
        }

        public IActionResult Create()
        {
            PopulateParty();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdParty,Name,PlaceDescription,Description,UpToDate,IdRegion")] Party party)
        {
            if (ModelState.IsValid)
            {
                await _partyRepository.AddPartyAsync(party);

                return RedirectToAction(nameof(Index));
            }

            PopulateParty(party.IdRegion);

            return View(party);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var party = await _partyRepository.GetPartyByIDWithoutInclude(id);

            if (party == null)
                    {
                return NotFound();
            }

            PopulateParty(party.IdRegion);

            return View(party);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partyToUpdate = await _partyRepository.GetPartyByIDWithoutIncludeAndAsNoTracking(id);

            if (await TryUpdateModelAsync<Party>(partyToUpdate,
                    "",
                    c => c.Name, c => c.PlaceDescription, c => c.Description, c => c.UpToDate, c => c.IdRegion))
            {
                try
                {
                    await _partyRepository.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError(String.Empty, "Nie można zapisać zmian.");
                }

                return RedirectToAction(nameof(Index));
            }

            PopulateParty(partyToUpdate.IdRegion);
            return View(partyToUpdate);
        }

        public void PopulateParty(object selectedRegion = null)
        {

            var regionQuery = from e in _partyRepository.GetAllRegionAsNoTracking()
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

            var party = await _partyRepository.GetPartyByID(id);

            if (party == null)
                    {
                return NotFound();
            }

            return View(party);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _partyRepository.GetPartyByIDWithoutIncludeAndAsNoTracking(id);
            await _partyRepository.DeletePartyAsync(course);

            return RedirectToAction(nameof(Index));
        }
    }
}