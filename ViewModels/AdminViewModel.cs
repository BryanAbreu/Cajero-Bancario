using Database.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels
{
    public class AdminViewModel
    {
        public List<Transacciones> Transacciones { get; set; }
        public int CantidadTransacciones { get; set; }
        public int TotalTransacciones { get; set; }
        public int CantidadProductos { get; set; }

        public List<Usuario> Usuarios {get; set;}

        public List<Productos> Productos { get; set; }
    }
}
