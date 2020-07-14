using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Database.Models;
using Email;
using Ibanking.Infraestructure.AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository.Repository;

namespace Ibanking
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<IbankingContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("Default")));

            var emailConfig = Configuration.GetSection("EmailConfig").Get<EmailConfig>();

            services.AddSingleton(emailConfig);

            services.AddAutoMapper(typeof(AutoMapperConfiguration).GetTypeInfo().Assembly);

            services.AddIdentity<IdentityUser, IdentityRole>(options => {

                options.Password = new PasswordOptions
                {
                    RequireDigit = true,
                    RequiredLength = 6,
                    RequireLowercase = false,
                    RequireUppercase = false,
                    RequireNonAlphanumeric = false
                };
            
            }).AddEntityFrameworkStores<IbankingContext>().AddDefaultTokenProviders();

            services.AddScoped<IEmailSender, GmailSender>();
            services.AddScoped<AdminRepository>();
            services.AddScoped<ClientRepository>();
            services.AddScoped<ProductosRepository>();
            services.AddScoped<BeneficiarioRepository>();
            services.AddScoped<PagosRepository>();
            services.AddScoped<TransaccionesRepository>();

           

            services.AddSession(so =>
            {
                so.IdleTimeout = TimeSpan.FromHours(2);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

           

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
