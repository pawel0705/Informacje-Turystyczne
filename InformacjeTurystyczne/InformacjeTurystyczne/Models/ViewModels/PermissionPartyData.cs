using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.ViewModels
{
    public class PermissionPartyData
    {
        public int IdParty { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }
}
