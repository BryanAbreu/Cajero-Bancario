using Database.Models;
using Microsoft.EntityFrameworkCore;
using RepositoryBase.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class BeneficiarioRepository : RepositoryBase<Beneficiario, IbankingContext>
    {
        private readonly IbankingContext _context;
        public BeneficiarioRepository(IbankingContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Beneficiario> Buscar(string codigo, string usuario)
        {
            var bene = await _context.Productos.FirstOrDefaultAsync(c => c.Cuenta == codigo && c.Producto == "Cuenta de Ahorro");
            if (bene == null)
            {
                return null;
            }
            else
            {
                var user = await _context.Usuarios.FirstOrDefaultAsync(c => c.UserName == bene.Usuario);
                if(user == null)
                {
                    return null;
                }
                var ben = new Beneficiario
                {
                    Nombre = user.Nombre,
                    Apellido = user.Apellido,
                    numeroCuenta = bene.Cuenta,
                    Usuario = usuario
                };
                return await Add(ben);
            }
            
            
        }
    }
}
