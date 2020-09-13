using InformacjeTurystyczne.Models.Tabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.InterfaceRepository
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllRegion();
        Task<Region> GetRegionByID(int? regionID);
        Task<Region> GetRegionByIDWithoutInclude(int? regionID);
        Task<Region> GetRegionByIDWithoutIncludeAndAsNoTracking(int? regionID);

        Task AddRegionAsync(Region region);
        void EditRegion(Region region);
        Task DeleteRegionAsync(Region region);

        Task SaveChangesAsync();
        IEnumerable<Region> GetAllRegionAsNoTracking();
        IEnumerable<Trail> GetAllTrails();
        void RemoveTrail(RegionLocation regionLocation);
        IEnumerable<Region> GetAllRegionToUser();
    }
}
