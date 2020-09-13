using InformacjeTurystyczne.Models.Tabels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InformacjeTurystyczne.Models.InterfaceRepository;

namespace InformacjeTurystyczne.Models.Repository
{
    public class RegionLocationRepository : IRegionLocationRepository
    {
        private readonly AppDbContext _appDbContext;

        public RegionLocationRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<RegionLocation> GetAllRegionLocation()
        {
            return _appDbContext.RegionLocations;
        }

        public RegionLocation GetRegionLocationByID(int regionLocationlID)
        {
            return _appDbContext.RegionLocations.FirstOrDefault(s => s.IdRegion == regionLocationlID);
        }

        public void AddRegionLocation(RegionLocation regionLocation)
        {
            _appDbContext.RegionLocations.Add(regionLocation);
            _appDbContext.SaveChanges();
        }

        public void DeleteRegionLocation(RegionLocation regionLocation)
        {
            _appDbContext.RegionLocations.Remove(regionLocation);
            _appDbContext.SaveChanges();
        }

        public void EditRegionLocation(RegionLocation regionLocation)
        {
            _appDbContext.RegionLocations.Update(regionLocation);
            _appDbContext.SaveChanges();
        }

    }
}
