using InformacjeTurystyczne.Models.Tabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.InterfaceRepository
{
    public interface IAttractionRepository
    {
        Task<IEnumerable<Attraction>> GetAllAttraction();
        Task<Attraction> GetAttractionByID(int? attractionID);
        Task<Attraction> GetAttractionByIDWithoutInclude(int? attractionID);
        Task<Attraction> GetAttractionByIDWithoutIncludeAndAsNoTracking(int? attractionID);

        Task AddAttractionAsync(Attraction attraction);
        void EditAttraction(Attraction attraction);
        Task DeleteAttractionAsync(Attraction attraction);

        Task SaveChangesAsync();
        IEnumerable<Attraction> GetAllAttractionAsNoTracking();
        IEnumerable<Region> GetAllRegionAsNoTracking();
        IEnumerable<Attraction> GetAllAttractionToUser();
    }
}
