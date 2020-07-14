using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public class Productos
    {
        public int Id { get; set; }
        public string Producto { get; set; }
        public string Usuario { get; set; }
        public string Cuenta { get; set; }
        public int Monto { get; set; }
        public string Tipo { get; set; }
    }
}
