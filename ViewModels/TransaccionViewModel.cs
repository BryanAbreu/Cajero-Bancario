using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels
{
    public class TransaccionViewModel
    {
        public int Id { get; set; }
        public string Cuenta { get; set; }
        public int Monto { get; set; }
        public string Destinatario { get; set; }
    }
}
