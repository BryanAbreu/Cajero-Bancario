using Database.Models;
using Microsoft.EntityFrameworkCore;
using RepositoryBase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Repository.Repository
{
    public class ClientRepository : RepositoryBase<Usuario, IbankingContext>
    {
        private readonly IbankingContext _context;
        public ClientRepository(IbankingContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ProductoViewModel>> getIndex(string user)
        {
            var prop = await _context.Productos.Where(c => c.Usuario == user).ToListAsync();
            List<ProductoViewModel> vm = new List<ProductoViewModel>();
            prop.ForEach(item =>
            {
                vm.Add(new ProductoViewModel
                {
                    Id = item.Id,
                    Cuenta = item.Cuenta,
                    Monto = item.Monto,
                    Producto = item.Producto,
                    Tipo = item.Tipo,
                    Usuario = item.Usuario
                });
            });
            return vm;

        }

        public async Task<List<BeneficiarioViewModel>> getBen(string user)
        {
            var bene =  await _context.Beneficiarios.Where(c => c.Usuario == user).ToListAsync();
            List<BeneficiarioViewModel> vm = new List<BeneficiarioViewModel>();
            bene.ForEach(item =>
            {
                vm.Add(new BeneficiarioViewModel
                {
                    Id = item.Id,
                    Usuario = item.Usuario,
                    Nombre = item.Nombre,
                    Apellido = item.Apellido,
                    numeroCuenta = item.numeroCuenta
                });

            });
            return vm;
        }
    }
}
