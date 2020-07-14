using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Database.Models;
using Email;
using ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Ibanking.Controllers
{
     
    public class AccountController : Controller
    {
        private readonly IbankingContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public static string ConfirmarCodigo { get; set; }

        public AccountController(IbankingContext context, IEmailSender emailSender, IMapper mapper, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _emailSender = emailSender;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login"); 
        } 


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Admin");
            }
            var user = await _userManager.FindByNameAsync(vm.User);
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(c => c.UserName == vm.User);
            if (ModelState.IsValid)
            {
                if(user == null)
                {
                    ModelState.AddModelError("NoUser", "Usuario no Existente");
                }
                else
                {
                    if (await _userManager.GetAccessFailedCountAsync(user) > 2)
                    {
                        usuario.Estado = "Inactivo";
                        _context.Usuarios.Update(usuario);
                        await _userManager.SetLockoutEnabledAsync(user, true);
                        await _context.SaveChangesAsync();
                        ModelState.AddModelError("UserDeactivated", "Usuario Inactivo, resuelva con el admin");

                    }
                    else if(usuario.Estado == "Inactivo")
                    {
                        ModelState.AddModelError("UserDeactivated", "Usuario Inactivo, resuelva con el admin");
                    }
                    else
                    {
                        var result = await _signInManager.PasswordSignInAsync(vm.User, vm.Password, false, false);

                        if (result.Succeeded)
                        {
                            if (await _userManager.IsInRoleAsync(user, "admin"))
                            {
                                return RedirectToAction("Index", "Admin");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Client");
                            }


                        }
                        ModelState.AddModelError("UserOrPasswordInvalid", "Usuario o Contraseña inválida");
                        await _userManager.AccessFailedAsync(user);
                    }
                }
               
                
               
            
            }
            
            return View(vm);
        }



        public async Task<IActionResult> Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Admin");
            }

            var roles = await _roleManager.Roles?.ToListAsync();
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
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
           
            if (ModelState.IsValid)
            {
                var users = await _context.Usuarios.FirstOrDefaultAsync(c => c.UserName == vm.UserName);
                var correo = await _context.Usuarios.FirstOrDefaultAsync(c => c.Correo == vm.Correo);
                if (users != null)
                {
                    ModelState.AddModelError("U/Exists", "Usuario ya existente");
                    return View(vm);
                }
                else if (correo != null)
                {
                    ModelState.AddModelError("C/Exists", "Correo ya usado por otro usuario");
                    return View(vm);
                }
                var usuario = _mapper.Map<Usuario>(vm);
                usuario.UserType = "client";
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                
                var user = new IdentityUser { UserName = vm.UserName, Email = vm.Correo};
                user.LockoutEnabled = true;
                var result = await _userManager.CreateAsync(user, vm.Password);

               

                if (result.Succeeded)
                {
                    var resultRol = await _userManager.AddToRoleAsync(user, vm.SelectedRol);

                    if (resultRol.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        return RedirectToAction("Index","Client");
                    }
                    
                }
                
                addErrors(result);   
            }
            return View(vm);
        }

        private void addErrors(IdentityResult result)
        {
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }


      

        public async Task<IActionResult> AccessDenied()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (await _userManager.IsInRoleAsync(user, "admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Client");
            }
        }


    }
}