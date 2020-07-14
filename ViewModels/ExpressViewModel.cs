using Database.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels
{
    public class ExpressViewModel
    {
        public string Cuenta { get; set; }
        public int Monto { get; set; }
        public string Destino { get; set; }

        public List<Productos> Productos { get; set; }
    }
}
