using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InformacjeTurystyczne.Models.InterfaceRepository;
using InformacjeTurystyczne.Models.Tabels;

namespace InformacjeTurystyczne.Models.ViewModels
{
    public class NotificationsVM
    {
        public IMessageRepository messages { get; set; }
        public List<Region> regions { get; set; }

        public AppUser user { get; set; }
    }
}
