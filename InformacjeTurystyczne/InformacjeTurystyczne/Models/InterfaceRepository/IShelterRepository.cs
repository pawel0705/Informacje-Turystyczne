using InformacjeTurystyczne.Models.Tabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.InterfaceRepository
{
    public interface IShelterRepository
    {
        Task<IEnumerable<Shelter>> GetAllShelter();
        Task<Shelter> GetShelterByID(int? shelterID);
        Task<Shelter> GetShelterByIDWithoutInclude(int? shelterID);
        Task<Shelter> GetShelterByIDWithoutIncludeAndAsNoTracking(int? shelterID);

        Task AddShelterAsync(Shelter shelter);
        void EditShelter(Shelter shelter);
        Task DeleteShelterAsync(Shelter shelter);

        Task SaveChangesAsync();
        IEnumerable<Shelter> GetAllShelterAsNoTracking();
        IEnumerable<Region> GetAllRegionAsNoTracking();
        IEnumerable<Shelter> GetAllShelterToUser();
    }
}
