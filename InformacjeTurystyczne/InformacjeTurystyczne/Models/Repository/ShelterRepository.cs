using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InformacjeTurystyczne.Models.InterfaceRepository;
using InformacjeTurystyczne.Models.Tabels;
using Microsoft.EntityFrameworkCore;

namespace InformacjeTurystyczne.Models.Repository
{
    public class ShelterRepository : IShelterRepository
    {
        public readonly AppDbContext _appDbContext;

        public ShelterRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Shelter>> GetAllShelter()
        {
            return await _appDbContext.Shelters.Include(c => c.Region).AsNoTracking().ToListAsync();
        }

        public async Task<Shelter> GetShelterByID(int? shelterID)
        {
            return await _appDbContext.Shelters.Include(c => c.Region).AsNoTracking().FirstOrDefaultAsync(s => s.IdShelter == shelterID);
        }

        public async Task<Shelter> GetShelterByIDWithoutInclude(int? shelterID)
        {
            return await _appDbContext.Shelters.AsNoTracking().FirstOrDefaultAsync(c => c.IdShelter == shelterID);
        }

        public async Task<Shelter> GetShelterByIDWithoutIncludeAndAsNoTracking(int? shelterID)
        {
            return await _appDbContext.Shelters.FirstOrDefaultAsync(c => c.IdShelter == shelterID);
        }

        public async Task AddShelterAsync(Shelter shelter)
        {
            _appDbContext.Shelters.Add(shelter);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteShelterAsync(Shelter shelter)
        {
            _appDbContext.Shelters.Remove(shelter);
            await _appDbContext.SaveChangesAsync();
        }

        public void EditShelter(Shelter shelter)
        {
            _appDbContext.Shelters.Update(shelter);
            _appDbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public IEnumerable<Shelter> GetAllShelterAsNoTracking()
        {
            return _appDbContext.Shelters.AsNoTracking();
        }

        public IEnumerable<Region> GetAllRegionAsNoTracking()
        {
            return _appDbContext.Regions.AsNoTracking();
        }

        public IEnumerable<Shelter> GetAllShelterToUser()
        {
            return _appDbContext.Shelters.Include(c => c.Region).AsNoTracking().ToList();
        }
    }
}
