using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels
{
    public class ProductoViewModel
    {
        public int Id { get; set; }
        public string Producto { get; set; }
        public string Usuario { get; set; }
        public string Cuenta { get; set; }
        public int Monto { get; set; }
        public string Tipo { get; set; }
    }
}
