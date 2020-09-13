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
    public class MessageController : Controller
    {
        private readonly IMessageRepository _messageRepository;

        public MessageController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<IActionResult> Index()
        {
            var messages = _messageRepository.GetAllMessage();

            return View(await messages);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messages = await _messageRepository.GetMessageByID(id);
            if (messages == null)
            {
                return NotFound();
            }

            return View(messages);
        }

        public IActionResult Create()
        {
            PopulateMessage();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMessage,Name,Description,PostingDate1,IdCategory,IdRegion")] Message message)
        {
            if (ModelState.IsValid)
            {
                await _messageRepository.AddMessageAsync(message);

                return RedirectToAction(nameof(Index));
            }

            PopulateMessage(message.IdRegion, message.IdCategory);

            return View(message);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _messageRepository.GetMessageByIDWithoutInclude(id);

            if (message == null)
            {
                return NotFound();
            }

            PopulateMessage(message.IdRegion, message.IdCategory);

            return View(message);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messageToUpdate = await _messageRepository.GetMessageByIDWithoutIncludeAndAsNoTracking(id);

            if (await TryUpdateModelAsync<Message>(messageToUpdate,
                    "",
                    c => c.Name, c => c.Description, c => c.PostingDate1))
            {
                try
                {
                    await _messageRepository.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError(String.Empty, "Nie można zapisać zmian.");
                }

                return RedirectToAction(nameof(Index));
            }

            PopulateMessage(messageToUpdate.IdRegion, messageToUpdate.IdCategory);
            return View(messageToUpdate);
        }

        public void PopulateMessage(object selectedRegion = null, object selectedCategory = null)
        {

            var regionQuery = from r in _messageRepository.GetAllRegionAsNoTracking()
                              orderby r.Name
                              select r;

            var categoryQuery = from c in _messageRepository.GetAllCategoryAsNoTracking()
                                orderby c.Name
                                select c;

            ViewBag.IdRegion = new SelectList(regionQuery, "IdRegion", "Name", selectedRegion);
            ViewBag.IdCategory = new SelectList(categoryQuery, "IdCategory", "Name", selectedCategory);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _messageRepository.GetMessageByID(id);

            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _messageRepository.GetMessageByIDWithoutIncludeAndAsNoTracking(id);
            await _messageRepository.DeleteMessageAsync(message);

            return RedirectToAction(nameof(Index));
        }
    }
}