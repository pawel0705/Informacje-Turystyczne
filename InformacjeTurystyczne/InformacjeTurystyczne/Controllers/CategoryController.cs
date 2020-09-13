using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using InformacjeTurystyczne.Models.InterfaceRepository;
using InformacjeTurystyczne.Models.Repository;
using InformacjeTurystyczne.Models.Tabels;
using Microsoft.EntityFrameworkCore;

namespace InformacjeTurystyczne.Controllers.TabelsController
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        private readonly IMessageRepository _messageRepository;

        public CategoryController(ICategoryRepository categoryRepository, IMessageRepository messageRepository)
        {
            _categoryRepository = categoryRepository;
            _messageRepository = messageRepository;
        }

        public async Task<IActionResult> Index()
        {
            var categorys = _categoryRepository.GetAllCategory();

            return View(await categorys);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categorys = await _categoryRepository.GetCategoryByID(id);
            if (categorys == null)
            {
                return NotFound();
            }

            return View(categorys);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategory,Name,PlaceDescription,Description,UpToDate,IdRegion")] Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.AddCategoryAsync(category);

                return RedirectToAction(nameof(Index));
            }


            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetCategoryByIDWithoutInclude(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryToUpdate = await _categoryRepository.GetCategoryByIDWithoutIncludeAndAsNoTracking(id);

            if (await TryUpdateModelAsync<Category>(categoryToUpdate,
                    "",
                    c => c.Name))
            {
                try
                {
                    await _categoryRepository.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError(String.Empty, "Nie można zapisać zmian.");
                }

                return RedirectToAction(nameof(Index));
            }

            return View(categoryToUpdate);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetCategoryByID(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _categoryRepository.GetCategoryByIDWithoutIncludeAndAsNoTracking(id);
            await _categoryRepository.DeleteCategoryAsync(category);

            return RedirectToAction(nameof(Index));
        }
    }
}