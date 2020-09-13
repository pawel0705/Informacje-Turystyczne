using System.ComponentModel.DataAnnotations;

namespace InformacjeTurystyczne.Models.ViewModels
{
    public class RegisterVM
    {
        [Required]
        [Display(Name="Nazwa użytkownika")]
        public string UserName { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail nie jest poprawny")]
        public string Email { get; set; }

        [Required]
        [Display(Name="Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
