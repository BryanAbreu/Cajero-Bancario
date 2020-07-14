using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModels
{
    public class RegisterViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Cedula")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public string Cedula { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public string Nombre { get; set; }

        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public string Apellido { get; set; }

        [Display(Name = "Correo")]
        [Required(ErrorMessage = "Este campo es requerido")]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "Este campo es requerido")]
        public string UserName { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Este campo es requerido")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirmar contraseña")]
        [Required(ErrorMessage = "Este campo es requerido")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "No coinciden las contraseñas")]
        public string ConfirmPassword { get; set; }

        public List<RolesViewModel> Roles { get; set; }

        [Display(Name = "Perfil")]
        public string SelectedRol { get; set; }

        public int Monto { get; set; }
        public string Estado { get; set; }
    }
}
