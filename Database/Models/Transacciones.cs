using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class Transacciones
    {
        public int Id { get; set; }
        public string Cuenta { get; set; }
        public int Monto { get; set; }
        public string Destinatario { get; set; }
    }
}
