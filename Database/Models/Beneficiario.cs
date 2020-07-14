using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class Beneficiario
    {
        public int Id { get; set; }

        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string numeroCuenta { get; set; }
    }
}
