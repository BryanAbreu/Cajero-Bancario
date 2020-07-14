using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Repository;
using ViewModels;

namespace Ibanking.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {

        private readonly AdminRepository _repository;
        private readonly ProductosRepository _Prepository;
        private readonly IMapper _mapper;
        private readonly IbankingContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AdminController(AdminRepository repository, IbankingContext context, IMapper mapper, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            ProductosRepository Prepository)
        {
            _repository = repository;
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _Prepository = Prepository;
    }
        public async Task<IActionResult>Index(){return View(await _repository.getAdminAll());}
        public async Task<IActionResult>EstadoUsuario(int? id){
            await _repository.EstadoUsuario(id.Value);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(c => c.Id == id);
            return View(await _repository.Edit(user));
        }

        public async Task<IActionResult> Create()
        {

            var roles = await _context.Roles?.ToListAsync();
            var rolesVm = new List<RolesViewModel>();

            roles.ForEach(c =>
            {
                rolesVm.Add(new RolesViewModel
                {
                    Name = c.Name,
                    RoleName = c.NormalizedName
                });
            });

            var vm = new RegisterViewModel { Roles = rolesVm };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Create(RegisterViewModel vm)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var users = await _context.Usuarios.FirstOrDefaultAsync(c => c.UserName == vm.UserName || c.Correo == vm.Correo);
                    var productos = _context.Productos.Where(c => c.Tipo == "Principal" && c.Cuenta == vm.UserName).Count();
                    if (users != null)
                    {
                        ModelState.AddModelError("U/Exists", "Usuario ya existente");
                        return View(vm);
                    }

                    var usuario = _mapper.Map<Usuario>(vm);
                    usuario.UserType = "client";
                    

                    var user = new IdentityUser { UserName = vm.UserName, Email = vm.Correo };
                    user.LockoutEnabled = true;
                    var result = await _userManager.CreateAsync(user, vm.Password);

                    var resultRol = await _userManager.AddToRoleAsync(user, vm.SelectedRol);

                    if (result.Succeeded)
                    {
                        if (productos < 1)
                        {
                            var prod = new Productos
                            {

                                Producto = "Cuenta de Ahorro",
                                Monto = vm.Monto,
                                Cuenta = new Random().Next(100000000, 999999999).ToString(),
                                Tipo = "Principal",
                                Usuario = vm.UserName
                            };
                            await _Prepository.Add(prod);
                        }

                        _context.Usuarios.Add(usuario);
                        await _context.SaveChangesAsync();



                        if (resultRol.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);

                            return RedirectToAction("Index", "Client");
                        }

                    }
                    else
                    {
                        addErrors(result);
                    }
                }
                catch
                {
                    return View(vm);
                }
                
                

            }
            return View(vm);
        }

        private void addErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        public async Task<IActionResult> UpdateUser(RegisterViewModel vm)
        {
           
            var userUp = _mapper.Map<Usuario>(vm);
            userUp.Monto += vm.Monto;
            userUp.UserType = vm.SelectedRol;
            userUp.Estado = "Activo";
            if (await _repository.CambiarPassword(vm))
            {
                await _repository.Update(userUp);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Edit");
            }
            

        }
      


    }
}