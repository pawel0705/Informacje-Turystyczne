using InformacjeTurystyczne.Models.InterfaceRepository;
using InformacjeTurystyczne.Models.Tabels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly AppDbContext _appDbContext;

        public TrailRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Trail>> GetAllTrail()
        {
            return await _appDbContext.Trails.AsNoTracking().ToListAsync();
        }

        public async Task<Trail> GetTrailByID(int? trailID)
        {
            return await _appDbContext.Trails.AsNoTracking().FirstOrDefaultAsync(s => s.IdTrail == trailID);
        }

        public async Task<Trail> GetTrailByIDWithoutInclude(int? trailID)
        {
            return await _appDbContext.Trails.AsNoTracking().FirstOrDefaultAsync(c => c.IdTrail == trailID);
        }

        public async Task<Trail> GetTrailByIDWithoutIncludeAndAsNoTracking(int? trailID)
        {
            return await _appDbContext.Trails.FirstOrDefaultAsync(c => c.IdTrail == trailID);
        }

        public async Task AddTrailAsync(Trail trail)
        {
            _appDbContext.Trails.Add(trail);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteTrailAsync(Trail trail)
        {
            _appDbContext.Trails.Remove(trail);
            await _appDbContext.SaveChangesAsync();
        }

        public void EditTrail(Trail trail)
        {
            _appDbContext.Trails.Update(trail);
            _appDbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public IEnumerable<Trail> GetAllTrailAsNoTracking()
        {
            return _appDbContext.Trails.AsNoTracking();
        }

        public IEnumerable<Trail> GetAllTrailToUser()
        {
            return _appDbContext.Trails.AsNoTracking()
                .Include(t => t.RegionLocation)
                .ToList();
        }

    }
}
