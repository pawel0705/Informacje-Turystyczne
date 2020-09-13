using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InformacjeTurystyczne.Models.InterfaceRepository;
using InformacjeTurystyczne.Models.Repository;

namespace InformacjeTurystyczne.Models.ViewModels
{
    public class TouristInformationVM
    {
        public IAttractionRepository attractions { get; set; }
        public IPartyRepository parties { get; set; }
        public IShelterRepository shelters { get; set; }
        public ITrailRepository trails { get; set; }
        public IRegionRepository regions { get; set; }
    }
}
