using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InformacjeTurystyczne.Models.Tabels
{
    public class Category
    {
        [Key]
        [Display(Name = "Klucz kategoria")]
        public int IdCategory { get; set; } // K

        [Display(Name = "Nazwa kategorii")]
        public string Name { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
