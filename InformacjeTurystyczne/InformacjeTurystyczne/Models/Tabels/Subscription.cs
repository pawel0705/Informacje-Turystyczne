using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.Tabels
{
    public class Subscription
    {
        [Key]
        [Display(Name = "Klucz subskrypcja")]
        public int IdSubscription { get; set; }

        [Display(Name = "Zasubskrybowano")]
        public bool IsSubscribed { get; set; }

        [Display(Name = "Klucz obcy region")]
        public int IdRegion { get; set; }

        [Display(Name = "Region")]
        public virtual Region Region { get; set; }

        [Display(Name = "Klucz obcy użytkownik")]
        public string IdUser { get; set; }

        [Display(Name = "Użytkownik")]
        public virtual AppUser User { get; set; }
    }
}
