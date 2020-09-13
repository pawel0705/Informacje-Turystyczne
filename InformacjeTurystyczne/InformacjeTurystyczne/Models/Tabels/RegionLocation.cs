using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.Tabels
{
    public class RegionLocation
    {
        public int? IdTrail { get; set; }
        public Trail Trail { get; set; }

        public int? IdRegion { get; set; }
        public Region Region { get; set; }
    }
}
