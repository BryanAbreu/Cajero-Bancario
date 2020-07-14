using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Correo { get; set; }
        public string UserName { get; set; }
        public string UserType { get; set; }
        public int Monto { get; set; }
        public string Estado { get; set; }
    }
}