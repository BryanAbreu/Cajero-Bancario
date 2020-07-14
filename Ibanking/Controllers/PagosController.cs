using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Repository;
using ViewModels;

namespace Ibanking.Controllers
{
    public class PagosController : Controller
    {
        private readonly PagosRepository _prepository;
        private readonly IbankingContext _context;
        private readonly ClientRepository _repository;
        private readonly BeneficiarioRepository _brepository;

        public PagosController(PagosRepository prepository, IbankingContext context, ClientRepository repository,
             BeneficiarioRepository brepository)
        {
            _prepository = prepository;
            _context = context;
            _repository = repository;
            _brepository = brepository;
        }


        public IActionResult Index(ExpressViewModel vm)
        {
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Express(ExpressViewModel vm)
        {
            if(await _prepository.PagoExpress(vm))
            {
                return RedirectToAction("Index","Client");
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Tarjeta(ExpressViewModel vm)
        {
            var tarjetas = await _context.Productos.Where(c => c.Producto == "Tarjeta de Credito"&& c.Usuario == User.Identity.Name).ToListAsync();
            vm.Productos = tarjetas;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> TarjetaCredito(ExpressViewModel vm)
        {

           if(await _prepository.TarjetaCredito(vm))
            {
                return RedirectToAction("Index","Client");
            }
            ModelState.AddModelError("U/Exists", "Usuario ya existente");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Prestamo(ExpressViewModel vm)
        {
            var prestamos = await _context.Productos.Where(c => c.Producto == "Prestamo" && c.Usuario == User.Identity.Name).ToListAsync();
            vm.Productos = prestamos;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> PagarPrestamo(ExpressViewModel vm)
        {

            if (await _prepository.Prestamo(vm))
            {
                return RedirectToAction("Index", "Client");
            }
            ModelState.AddModelError("U/Exists", "Usuario ya existente");
            return RedirectToAction("Index");
        }

     
    }
}