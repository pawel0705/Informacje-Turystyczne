using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.Tabels
{
    public class Attraction
    {
        [Key]
        [Display(Name = "Klucz atrakcje")]
        public int IdAttraction { get; set; } // K

        [Display(Name = "Typ atrakcji")]
        public string AttractionType { get; set; }

        [Display(Name = "Nazwa atrakcji")]
        public string Name { get; set; }

        [Display(Name = "Opis atrakcji")]
        public string Description { get; set; }

        [Display(Name = "Klucz obcy region")]
        public int IdRegion { get; set; }

        [Display(Name = "Region")]
        public virtual Region Region { get; set; }
    }
}
