using InformacjeTurystyczne.Models.Tabels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models
{
    public class AppUser : IdentityUser
    {
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual ICollection<PermissionParty> PermissionPartys { get; set; }
        public virtual ICollection<PermissionRegion> PermissionRegions { get; set; }
        public virtual ICollection<PermissionShelter> PermissionShelters { get; set; }
        public virtual ICollection<PermissionTrail> PermissionTrails { get; set; }
    }
}
