using InformacjeTurystyczne.Models.Tabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.InterfaceRepository
{
    public interface ISubscriptionRepository
    {
        Task<IEnumerable<Subscription>> GetAllSubscription();
        Task<Subscription> GetSubscriptionByID(int? subscriptionID);
        Task<Subscription> GetSubscriptionByIDWithoutInclude(int? subscriptionID);
        Task<Subscription> GetSubscriptionByIDWithoutIncludeAndAsNoTracking(int? subscriptionID);

        Task AddSubscriptionAsync(Subscription subscription);
        void EditSubscription(Subscription subscription);
        Task DeleteSubscriptionAsync(Subscription subscription);

        Task SaveChangesAsync();
        IEnumerable<Subscription> GetAllSubscriptionAsNoTracking();
        IEnumerable<Region> GetAllRegionAsNoTracking();
        IEnumerable<AppUser> GetAllAppUserAsNoTracking();
        IEnumerable<Subscription> GetAllSubscriptionToUser();
    }
}
