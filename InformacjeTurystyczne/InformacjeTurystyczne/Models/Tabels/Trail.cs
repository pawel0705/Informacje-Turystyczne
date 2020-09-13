using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.Tabels
{
    public class Trail
    {
        [Key]
        [Display(Name = "Klucz szlak")]
        public int IdTrail { get; set; } // K

        [Display(Name = "Nazwa szlaku")]
        public string Name { get; set; }

        [Display(Name = "Kolor")]
        public string Colour { get; set; }

        [Display(Name = "Otwarty")]
        public bool Open { get; set; }

        [Display(Name = "Przyczyna zamknięcia")]
        public string Feedback { get; set; }

        [Display(Name = "Długość")]
        public float Length { get; set; }

        [Display(Name = "Trudność")]
        public int Difficulty { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        public virtual ICollection<RegionLocation> RegionLocation { get; set; }
        public virtual ICollection<PermissionTrail> PermissionTrail { get; set; }
    }
}
