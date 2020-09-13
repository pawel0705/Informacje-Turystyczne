using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.Tabels
{
    public class PermissionRegion
    {
        public int? IdRegion { get; set; }
        public Region Region { get; set; }

        public string IdUser { get; set; }
        public AppUser User { get; set; }
    }
}
