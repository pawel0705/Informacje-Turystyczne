using InformacjeTurystyczne.Models.Tabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.InterfaceRepository
{
    public interface ITrailRepository
    {
        Task<IEnumerable<Trail>> GetAllTrail();
        Task<Trail> GetTrailByID(int? trailID);
        Task<Trail> GetTrailByIDWithoutInclude(int? trailID);
        Task<Trail> GetTrailByIDWithoutIncludeAndAsNoTracking(int? trailID);

        Task AddTrailAsync(Trail trail);
        void EditTrail(Trail trail);
        Task DeleteTrailAsync(Trail trail);

        Task SaveChangesAsync();
        IEnumerable<Trail> GetAllTrailAsNoTracking();
        IEnumerable<Trail> GetAllTrailToUser();
    }

}
