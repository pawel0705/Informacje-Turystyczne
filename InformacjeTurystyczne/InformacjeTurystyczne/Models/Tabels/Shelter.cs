using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.Tabels
{
    public class Shelter
    {
        [Key]
        [Display(Name = "Klucz schronisko")]
        public int IdShelter { get; set; }

        [Display(Name = "Nazwa schroniska")]
        public string Name { get; set; }

        [Display(Name = "Max il. miejsc")]
        public int MaxPlaces { get; set; }

        [Display(Name = "Miejsca")]
        public int Places { get; set; }

        [Display(Name = "Otwarte")]
        public bool IsOpen { get; set; }

        [Display(Name = "Numer telefonu")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "Klucz obcy region")]
        public int IdRegion { get; set; }

        [Display(Name = "Region")]
        public virtual Region Region { get; set; }

        public virtual ICollection<PermissionShelter> PermissionShelters { get; set; }
    }
}
