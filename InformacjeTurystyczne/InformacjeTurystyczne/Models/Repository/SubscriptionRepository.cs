using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InformacjeTurystyczne.Models.InterfaceRepository;
using InformacjeTurystyczne.Models.Tabels;
using Microsoft.EntityFrameworkCore;

namespace InformacjeTurystyczne.Models.Repository
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        public readonly AppDbContext _appDbContext;

        public SubscriptionRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Subscription>> GetAllSubscription()
        {
            return await _appDbContext.Subscriptions.Include(s=>s.Region).Include(s=>s.User).AsNoTracking().ToListAsync();
        }

        public async Task<Subscription> GetSubscriptionByID(int? subscriptionID)
        {
            return await _appDbContext.Subscriptions.Include(s => s.Region).Include(s => s.User).AsNoTracking().FirstOrDefaultAsync(s => s.IdSubscription == subscriptionID);
        }

        public async Task<Subscription> GetSubscriptionByIDWithoutInclude(int? subscriptionID)
        {
            return await _appDbContext.Subscriptions.AsNoTracking().FirstOrDefaultAsync(c => c.IdSubscription == subscriptionID);
        }

        public async Task<Subscription> GetSubscriptionByIDWithoutIncludeAndAsNoTracking(int? subscriptionID)
        {
            return await _appDbContext.Subscriptions.FirstOrDefaultAsync(c => c.IdSubscription == subscriptionID);
        }

        public async Task AddSubscriptionAsync(Subscription subscription)
        {
            _appDbContext.Subscriptions.Add(subscription);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteSubscriptionAsync(Subscription subscription)
        {
            _appDbContext.Subscriptions.Remove(subscription);
            await _appDbContext.SaveChangesAsync();
        }

        public void EditSubscription(Subscription subscription)
        {
            _appDbContext.Subscriptions.Update(subscription);
            _appDbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public IEnumerable<Subscription> GetAllSubscriptionAsNoTracking()
        {
            return _appDbContext.Subscriptions.AsNoTracking();
        }

        public IEnumerable<Region> GetAllRegionAsNoTracking()
        {
            return _appDbContext.Regions.AsNoTracking();
        }

        public IEnumerable<AppUser> GetAllAppUserAsNoTracking()
        {
            return _appDbContext.Users.AsNoTracking();
        }

        public IEnumerable<Subscription> GetAllSubscriptionToUser()
        {
            return _appDbContext.Subscriptions.Include(s => s.Region).Include(s => s.User).AsNoTracking().ToList();
        }
    }
}
