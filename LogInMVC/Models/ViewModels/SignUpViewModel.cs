using System.ComponentModel.DataAnnotations;

namespace LogInMVC.Models.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede tener más de 50 caracteres.")]
        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "La constraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Debe confirmar la contraseña.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña deben coincidir.")]
        public string ConfirmPassword { get; set; }
    }
}
