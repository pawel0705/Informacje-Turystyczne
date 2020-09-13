using InformacjeTurystyczne.Models.Tabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InformacjeTurystyczne.Models.InterfaceRepository;

namespace InformacjeTurystyczne.Models.Repository
{
    public class AttractionRepository : IAttractionRepository
    {
        public readonly AppDbContext _appDbContext;

        public AttractionRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Attraction>> GetAllAttraction()
        {
            return await _appDbContext.Attractions.Include(c => c.Region).AsNoTracking().ToListAsync();
        }

        public async Task<Attraction> GetAttractionByID(int? attractionID)
        {
            return await _appDbContext.Attractions.Include(c => c.Region).AsNoTracking().FirstOrDefaultAsync(s => s.IdAttraction == attractionID);
        }

        public async Task<Attraction> GetAttractionByIDWithoutInclude(int? attractionID)
        {
            return await _appDbContext.Attractions.AsNoTracking().FirstOrDefaultAsync(c => c.IdAttraction == attractionID);
        }

        public async Task<Attraction> GetAttractionByIDWithoutIncludeAndAsNoTracking(int? attractionID)
        {
            return await _appDbContext.Attractions.FirstOrDefaultAsync(c => c.IdAttraction == attractionID);
        }

        public async Task AddAttractionAsync(Attraction attraction)
        {
            _appDbContext.Attractions.Add(attraction);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAttractionAsync(Attraction attraction)
        {
            _appDbContext.Attractions.Remove(attraction);
            await _appDbContext.SaveChangesAsync();
        }

        public void EditAttraction(Attraction attraction)
        {
            _appDbContext.Attractions.Update(attraction);
            _appDbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public IEnumerable<Attraction> GetAllAttractionAsNoTracking()
        {
            return _appDbContext.Attractions.AsNoTracking();
        }

        public IEnumerable<Region> GetAllRegionAsNoTracking()
        {
            return _appDbContext.Regions.AsNoTracking();
        }

        public IEnumerable<Attraction> GetAllAttractionToUser()
        {
            return _appDbContext.Attractions.Include(c => c.Region).AsNoTracking().ToList();
        }
    }
}
