using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.ViewModels
{
    public class PermissionRegionData
    {
        public int IdRegion { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }
}