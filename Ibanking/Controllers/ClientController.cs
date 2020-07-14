using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using Repository.Repository;
using ViewModels;

namespace Ibanking.Controllers
{


    [Authorize(Roles = "client")]
    public class ClientController : Controller
    {
        private readonly ProductosRepository _Prepository;
        private readonly ClientRepository _repository;
        private readonly BeneficiarioRepository _Brepository;
        private readonly IMapper _mapper;
        private readonly IbankingContext _context;
        public ClientController(ClientRepository repository,
            IMapper mapper, BeneficiarioRepository Brepository,
            IbankingContext context, ProductosRepository Prepository)
        {
            _repository = repository;
            _mapper = mapper;
            _Brepository = Brepository;
            _context = context;
            _Prepository = Prepository;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _repository.getIndex(User.Identity.Name));
        }


        public async Task<IActionResult> Beneficiario()
        {

            return View(await _repository.getBen(User.Identity.Name));
        }


        public async Task<IActionResult> DelBen(int id)
        {

            await _Brepository.Delete(id);
            return RedirectToAction("Beneficiario");
        }
        [HttpPost]
        public async Task<IActionResult> BuscarBen()
        {
            var codigo = Request.Form["busqueda"];
            if (await _Brepository.Buscar(codigo, User.Identity.Name) ==null)
            {
                ModelState.AddModelError("", "Beneficiario no encontrado");
            }
            else
            {
                return RedirectToAction("Beneficiario");
            }
                return RedirectToAction("Beneficiario");

        }
    }

    
}