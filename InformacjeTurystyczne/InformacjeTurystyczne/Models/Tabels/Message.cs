using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.Tabels
{
    public class Message
    {
        [Key]
        [Display(Name = "Klucz wiadomość")]
        public int IdMessage { get; set; } // K

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Treść")]
        public string Description { get; set; }

        [Display(Name = "Data zapostowania")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PostingDate1 { get; set; }

        [Display(Name = "Klucz obcy kategoria")]
        public int? IdCategory { get; set; }

        [Display(Name = "Kategoria")]
        public virtual Category Category { get; set; }

        [Display(Name = "Klucz obcy region")]
        public int? IdRegion { get; set; }

        [Display(Name = "Region")]
        public virtual Region Region { get; set; }
    }
}
