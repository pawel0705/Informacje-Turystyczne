using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.Tabels
{
    public class Region
    {
        [Key]
        [Display(Name = "Klucz region")]
        public int IdRegion { get; set; }

        [Display(Name = "Nazwa regionu")]
        public string Name { get; set; }

        public virtual ICollection<Message> Message { get; set; }
        public virtual ICollection<Party> Party { get; set; }
        public virtual ICollection<RegionLocation> RegionLocation { get; set; }
        public virtual ICollection<Shelter> Shelter { get; set; }
        public virtual ICollection<PermissionRegion> PermissionRegion {get; set;}
        public virtual ICollection<Subscription> Subscription { get; set; }
        public virtual ICollection<Attraction> Attraction { get; set; }
    }
}
