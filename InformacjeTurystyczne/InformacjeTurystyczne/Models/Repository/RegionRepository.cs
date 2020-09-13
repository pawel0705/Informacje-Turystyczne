using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using InformacjeTurystyczne.Models.InterfaceRepository;
using InformacjeTurystyczne.Models.Tabels;

namespace InformacjeTurystyczne.Models.Repository
{
    public class RegionRepository : IRegionRepository
    {
        private readonly AppDbContext _appDbContext;

        public RegionRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Region>> GetAllRegion()
        {
            return await _appDbContext.Regions
                .Include(i=>i.RegionLocation)
                .ThenInclude(i=>i.Trail)
                .AsNoTracking().ToListAsync();
        }

        public async Task<Region> GetRegionByID(int? regionID)
        {
            return await _appDbContext.Regions
                .Include(i=>i.RegionLocation)
                .ThenInclude(i=>i.Trail)
                .FirstOrDefaultAsync(s => s.IdRegion == regionID);
        }

        public async Task<Region> GetRegionByIDWithoutInclude(int? regionID)
        {
            return await _appDbContext.Regions.AsNoTracking().FirstOrDefaultAsync(c => c.IdRegion == regionID);
        }

        public async Task<Region> GetRegionByIDWithoutIncludeAndAsNoTracking(int? regionID)
        {
            return await _appDbContext.Regions.FirstOrDefaultAsync(c => c.IdRegion == regionID);
        }

        public async Task AddRegionAsync(Region region)
        {
            _appDbContext.Regions.Add(region);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteRegionAsync(Region region)
        {
            _appDbContext.Regions.Remove(region);
            await _appDbContext.SaveChangesAsync();
        }

        public void EditRegion(Region region)
        {
            _appDbContext.Regions.Update(region);
            _appDbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public IEnumerable<Region> GetAllRegionAsNoTracking()
        {
            return _appDbContext.Regions.AsNoTracking();
        }

        public IEnumerable<Trail> GetAllTrails()
        {
            return _appDbContext.Trails;
        }

        void IRegionRepository.RemoveTrail(RegionLocation regionLocation)
        {
            _appDbContext.Remove(regionLocation);
        }

        public IEnumerable<Region> GetAllRegionToUser()
        {
            return _appDbContext.Regions.AsNoTracking().ToList();
        }
    }
}
