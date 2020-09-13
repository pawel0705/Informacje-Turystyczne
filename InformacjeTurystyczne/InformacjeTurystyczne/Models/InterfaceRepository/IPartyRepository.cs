using InformacjeTurystyczne.Models.Tabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.InterfaceRepository
{
    public interface IPartyRepository
    {
        Task<IEnumerable<Party>> GetAllParty();
        Task<Party> GetPartyByID(int? partyID);
        Task<Party> GetPartyByIDWithoutInclude(int? partyID);
        Task<Party> GetPartyByIDWithoutIncludeAndAsNoTracking(int? partyID);

        Task AddPartyAsync(Party party);
        void EditParty(Party party);
        Task DeletePartyAsync(Party party);

        Task SaveChangesAsync();
        IEnumerable<Party> GetAllPartyAsNoTracking();
        IEnumerable<Region> GetAllRegionAsNoTracking();
        IEnumerable<Party> GetAllPartyToUser();
    }
}
