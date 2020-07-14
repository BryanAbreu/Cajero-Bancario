using Database.Models;
using Microsoft.AspNetCore.Identity;
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
    public class AdminRepository : RepositoryBase<Usuario,IbankingContext> 
    {
        private readonly IbankingContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public AdminRepository(IbankingContext context, UserManager<IdentityUser> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<Transacciones>> getTransacciones()
        {
           return  await _context.Transacciones.ToListAsync();
        }

        public async Task<List<Productos>> getProductos()
        {
            return await _context.Productos.ToListAsync();
        }

        public async Task<AdminViewModel> getAdminAll()
        {
            var listUsuarios = await getAll();
            var listTransacciones = await getTransacciones();
            var listProductos = await getProductos();
            var cantidadTransacciones =  CantidadTransacciones();
            var totalTransacciones = TotalTransacciones();
            var totalProductos = TotalProductos();

            var viewModel = new AdminViewModel
            {
                CantidadTransacciones = cantidadTransacciones,
                Usuarios = listUsuarios,
                Transacciones = listTransacciones,
                Productos = listProductos,
                CantidadProductos = totalProductos,
                TotalTransacciones = totalTransacciones

            };
            return viewModel;
        }
        public async Task<bool> EstadoUsuario(int id)
        {

            var nuevoEstado = "";
            var usuario = await getById(id);
            var user = await _userManager.FindByNameAsync(usuario.UserName);
            if (usuario == null)
            {
                return false;
            }

            if (usuario.Estado == "Inactivo")
            {
                nuevoEstado = "Activo";
            }
            else
            {
                nuevoEstado = "Inactivo";
            }
            usuario.Estado = nuevoEstado;
            await _userManager.ResetAccessFailedCountAsync(user);
            await Update(usuario);

            return true;
        }

        public int  TotalTransacciones()
        {
            var transacciones =  _context.Transacciones.Select(s=> s.Monto).Sum();
            return transacciones;


        }

        public int TotalProductos()
        {
            var productos = _context.Productos.Where(c => c.Cuenta != null).Count();
            return productos;
        }

        public int CantidadTransacciones()
        {
            var transacciones = _context.Transacciones.Where(c => c.Cuenta != null).Count();
            return transacciones;
        }

        public async Task<RegisterViewModel> Edit(Usuario user)
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

            var viewModel = new RegisterViewModel
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Cedula = user.Cedula,
                UserName = user.UserName,
                Correo = user.Correo,
                Monto = user.Monto,
                Estado = user.Estado,
                Roles = rolesVm
            };
            
            return viewModel;
        }

        public async Task<bool> CambiarPassword(RegisterViewModel viewModel)
        {
            var user = await _userManager.FindByNameAsync(viewModel.UserName);

            if (user == null)
            {

                return false;
            }
            else
            {

                    if (await _userManager.CheckPasswordAsync(user, viewModel.Password))
                    {
                        await _userManager.ChangePasswordAsync(user, viewModel.Password, viewModel.ConfirmPassword);
                        return true;
                    }
                   

            }

            return false;
        }

    }
}
