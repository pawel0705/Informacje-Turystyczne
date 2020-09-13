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
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionController(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public async Task<IActionResult> Index()
        {
            var subscriptions = _subscriptionRepository.GetAllSubscription();

            return View(await subscriptions);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscriptions = await _subscriptionRepository.GetSubscriptionByID(id);
            if (subscriptions == null)
            {
                return NotFound();
            }

            return View(subscriptions);
        }

        public IActionResult Create()
        {
            PopulateSubscription();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSubscription, IsSubscribed, IdUser, IdRegion")] Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                await _subscriptionRepository.AddSubscriptionAsync(subscription);

                return RedirectToAction(nameof(Index));
            }

            PopulateSubscription(subscription.IdRegion, subscription.IdUser);

            return View(subscription);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _subscriptionRepository.GetSubscriptionByIDWithoutInclude(id);

            if (subscription == null)
            {
                return NotFound();
            }

            PopulateSubscription(subscription.IdRegion, subscription.IdUser);

            return View(subscription);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscriptionToUpdate = await _subscriptionRepository.GetSubscriptionByIDWithoutIncludeAndAsNoTracking(id);

            if (await TryUpdateModelAsync<Subscription>(subscriptionToUpdate,
                    "",
                    c => c.IsSubscribed, c => c.IdRegion, c=>c.IdUser))
            {
                try
                {
                    await _subscriptionRepository.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError(String.Empty, "Nie można zapisać zmian.");
                }

                return RedirectToAction(nameof(Index));
            }

            PopulateSubscription(subscriptionToUpdate.IdRegion, subscriptionToUpdate.IdUser);
            return View(subscriptionToUpdate);
        }

        public void PopulateSubscription(object selectedRegion = null, object selectedUser = null)
        {

            var regionQuery = from e in _subscriptionRepository.GetAllRegionAsNoTracking()
                              orderby e.Name
                              select e;

            var userQuery = from e in _subscriptionRepository.GetAllAppUserAsNoTracking()
                              orderby e.UserName
                              select e;

            ViewBag.IdRegion = new SelectList(regionQuery, "IdRegion", "Name", selectedRegion);
            ViewBag.IdUser = new SelectList(userQuery, "Id", "UserName", selectedUser);

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _subscriptionRepository.GetSubscriptionByID(id);

            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subscription = await _subscriptionRepository.GetSubscriptionByIDWithoutIncludeAndAsNoTracking(id);
            await _subscriptionRepository.DeleteSubscriptionAsync(subscription);

            return RedirectToAction(nameof(Index));
        }
    }
}