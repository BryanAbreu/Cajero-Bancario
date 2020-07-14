using Database.Models;
using Microsoft.EntityFrameworkCore;
using RepositoryBase.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Repository.Repository
{
    public class PagosRepository : RepositoryBase<Usuario, IbankingContext>
    {
        private readonly IbankingContext _context;
        private readonly TransaccionesRepository _trepository;
        private readonly ClientRepository _repository;
        public PagosRepository(IbankingContext context, TransaccionesRepository trepository,
            ClientRepository repository) : base(context)
        {
            _context = context;
            _trepository = trepository;
            _repository = repository;
        }


        public  async Task<bool> PagoExpress(ExpressViewModel vm)
        {
            var userA = await _context.Productos.FirstOrDefaultAsync(c => c.Cuenta == vm.Cuenta);
            if(userA.Monto > vm.Monto)
            {
                var userB = await _context.Productos.FirstOrDefaultAsync(c => c.Cuenta == vm.Destino);
                if(userB == null)
                {
                    return false;
                }
                userA.Monto -= vm.Monto;
                userB.Monto += vm.Monto;
                var recibo = new Transacciones
                {
                    Cuenta = vm.Cuenta,
                    Monto = vm.Monto,
                    Destinatario = vm.Destino
                };
                await _trepository.Add(recibo);
                return true;
                
          
            }
            return false;
        }

        public async Task<bool> TarjetaCredito(ExpressViewModel vm)
        {
            var userA = await _context.Productos.FirstOrDefaultAsync(c => c.Cuenta == vm.Cuenta && c.Producto=="Tarjeta de Credito");
            if(userA == null)
            {
                return false;
            }

            if (userA.Monto > vm.Monto || userA.Monto >0)
            {
                var userB = await _context.Productos.FirstOrDefaultAsync(c => c.Cuenta == vm.Destino && c.Producto == "Tarjeta de Credito");
                if (userB == null)
                {
                    return false;
                }
                if(userA.Monto < userB.Monto)
                {
                    userA.Monto -= vm.Monto;
                    userB.Monto += vm.Monto;
                }
                else if(vm.Monto > userB.Monto  )
                {
                    userA.Monto -= userB.Monto;
                    
                }
                ;

                var recibo = new Transacciones
                {
                    Cuenta = vm.Cuenta,
                    Monto = vm.Monto,
                    Destinatario = vm.Destino
                };
                await _trepository.Add(recibo);
                return true;


            }
            return false;
        }

        public async Task<bool> Prestamo(ExpressViewModel vm)
        {
            var userA = await _context.Productos.FirstOrDefaultAsync(c => c.Cuenta == vm.Cuenta && c.Producto == "Prestamo");
            if (userA == null)
            {
                return false;
            }

            if (userA.Monto < vm.Monto || userA.Monto < 0)
            {
                var userB = await _context.Productos.FirstOrDefaultAsync(c => c.Cuenta == vm.Destino);
                if (userB == null)
                {
                    return false;
                }

                
                    userB.Monto -= vm.Monto;
                    userA.Monto += vm.Monto;

                

                var recibo = new Transacciones
                {
                    Cuenta = vm.Cuenta,
                    Monto = vm.Monto,
                    Destinatario = vm.Destino
                };
                await _trepository.Add(recibo);
                return true;


            }
            return false;
        }


    }
}
