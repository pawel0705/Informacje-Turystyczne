using InformacjeTurystyczne.Models.Tabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InformacjeTurystyczne.Models.InterfaceRepository;

namespace InformacjeTurystyczne.Models.Repository
{
    public class PartyRepository : IPartyRepository
    {
        public readonly AppDbContext _appDbContext;

        public PartyRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Party>> GetAllParty()
        {
            return await _appDbContext.Partys.Include(c => c.Region).AsNoTracking().ToListAsync();
        }

        public async Task<Party> GetPartyByID(int? partyID)
        {
            return await _appDbContext.Partys.Include(c => c.Region).AsNoTracking().FirstOrDefaultAsync(s => s.IdParty == partyID);
        }

        public async Task<Party> GetPartyByIDWithoutInclude(int? partyID)
        {
            return await _appDbContext.Partys.AsNoTracking().FirstOrDefaultAsync(c => c.IdParty == partyID);
        }

        public async Task<Party> GetPartyByIDWithoutIncludeAndAsNoTracking(int? partyID)
        {
            return await _appDbContext.Partys.FirstOrDefaultAsync(c => c.IdParty == partyID);
        }

        public async Task AddPartyAsync(Party party)
        {
            _appDbContext.Partys.Add(party);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeletePartyAsync(Party party)
        {
            _appDbContext.Partys.Remove(party);
            await _appDbContext.SaveChangesAsync();
        }

        public void EditParty(Party party)
        {
            _appDbContext.Partys.Update(party);
            _appDbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public IEnumerable<Party> GetAllPartyAsNoTracking()
        {
            return _appDbContext.Partys.AsNoTracking();
        }

        public IEnumerable<Region> GetAllRegionAsNoTracking()
        {
            return _appDbContext.Regions.AsNoTracking();
        }

        public IEnumerable<Party> GetAllPartyToUser()
        {
            return _appDbContext.Partys.Include(c => c.Region).AsNoTracking().ToList();
        }
    }
}
